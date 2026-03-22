using System.Formats.Asn1;
using URLShortner.Entities;

namespace URLShortner.Services
{
    public interface IUrlShortnerService 
    {
        Task<string> GetShortendUrl(string fullUrl);
        Task<string> GetFullUrl(string code);
    }
    public class UrlShortnerService : IUrlShortnerService
    {
        public const int MAX_LENGTH =  7;

        public const string Key = "ABCDEFGHIJKLMNOPQRSTUVWXZ1234567890abcdefghijklmnopqrstuvwxz";

        public static Dictionary<string, string> internalCache = new Dictionary<string, string>();

        public ApplicationDBContext _context;

        public UrlShortnerService(ApplicationDBContext context) 
        {
            _context = context;
        }

        public async Task<string> GetShortendUrl(string fullUrl) 
        {
            string url = string.Empty;

            Random random = new Random();
            while (true) 
            {
                for (int i = 0; i < MAX_LENGTH; i++) 
                {
                    int index = random.Next(Key.Length - 1);
                    url += Key[index];
                }

                if (!internalCache.ContainsKey(url)) 
                {
                    var data = await _context.UrlShorteners.FindAsync(url);

                    if (data == null) 
                    {
                        internalCache.Add(url, fullUrl);
                        UrlShortner urlShortner = new()
                        {
                            FullUrl = fullUrl,
                            ShortUrl = url,
                            CreatedDate = DateTime.UtcNow
                        };

                        await _context.UrlShorteners.AddAsync(urlShortner);
                        await _context.SaveChangesAsync();
                        return url;
                    }

                    internalCache.Add(data.ShortUrl, data.FullUrl);
                }
            }
        }

        public async Task<string> GetFullUrl(string code) 
        {
            if (internalCache.ContainsKey(code)) 
            {
                return internalCache[code];
            }

            var result = await _context.UrlShorteners.FindAsync(code);

            if (result != null) 
            {
                internalCache.Add(result.ShortUrl, result.FullUrl);
                return result.FullUrl;
            }

            return null;
        }
    }
}
