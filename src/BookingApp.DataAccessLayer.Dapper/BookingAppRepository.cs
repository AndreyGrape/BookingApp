using Dapper;
using System.Data;
using BookingApp.DataAccessLayer.Contracts;
using Microsoft.Extensions.Configuration;
using BookingApp.Contracts;

namespace BookingApp.DataAccessLayer.Dapper
{
    public class BookingAppRepository : IBookingAppRepository
    {
        private readonly BookingDbContext _context;
        private readonly ChangeVersion _versions;
        
        public BookingAppRepository(
            BookingDbContext context,
            IConfiguration configuration,
            ChangeVersion versions)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _versions = versions ?? throw new ArgumentNullException(nameof(versions));

            var config = configuration ?? throw new ArgumentNullException(nameof(configuration));
            var clientLastVersionStr = config?.GetSection("ChangeVersions")?["LastClientChangeVersion"] ?? string.Empty;
            var bookingLastVersionStr = config?.GetSection("ChangeVersions")?["LastBookingChangeVersion"] ?? string.Empty;
            var eventLastVersionStr = config?.GetSection("ChangeVersions")?["LastEventChangeVersion"] ?? string.Empty;

            if (_versions.LastClientVersion == 0 && long.TryParse(clientLastVersionStr, out var clientLastVersion))
            {
                _versions.LastClientVersion = clientLastVersion;
            }
            if (_versions.LastBookingVersion == 0 && long.TryParse(bookingLastVersionStr, out var bookingLastVersion))
            {
                _versions.LastBookingVersion = bookingLastVersion;
            }
            if (_versions.LastEventVersion == 0 && long.TryParse(eventLastVersionStr, out var eventLastVersion))
            {
                _versions.LastEventVersion = eventLastVersion;
            }
        }

        public async Task<IEnumerable<Client>> GetClientsChange(CancellationToken cancellationToken = default) => 
            await GetEntityChange<Client>("[dbo].[GetClientChange]", EntityType.Client, _versions.LastClientVersion, cancellationToken);

        public async Task<IEnumerable<Booking>> GetBookingsChange(CancellationToken cancellationToken = default) => 
            await GetEntityChange<Booking>("[dbo].[GetBookingChange]", EntityType.Booking, _versions.LastBookingVersion, cancellationToken);

        public async Task<IEnumerable<Event>> GetEventsChange(CancellationToken cancellationToken = default) => 
            await GetEntityChange<Event>("[dbo].[GetEventChange]", EntityType.Event, _versions.LastEventVersion, cancellationToken);

        private async Task<IEnumerable<TEntity>> GetEntityChange<TEntity>(
            string sqlText,
            EntityType entityType,
            long lastVersion,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            using var connection = _context.CreateConnection();

            var dynamicParams = new DynamicParameters();
            dynamicParams.Add(
                "lastVersion",
                value: lastVersion,
                dbType: DbType.Int64,
                direction: ParameterDirection.InputOutput);

            var entities = await connection.QueryAsync<TEntity>(
                new CommandDefinition(commandText: sqlText, parameters: dynamicParams, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));
            var nextVersion = dynamicParams.Get<long>("lastVersion");

            switch (entityType)
            {
                case EntityType.Client:
                    _versions.LastClientVersion = nextVersion;
                    break;
                case EntityType.Booking:
                    _versions.LastBookingVersion = nextVersion;
                    break;
                case EntityType.Event:
                    _versions.LastEventVersion = nextVersion;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null);
            }

            return entities;
        }
    }
}
