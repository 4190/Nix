namespace Shortener.Dtos
{
    public class ShortenedLinkDto
    {
        public int Id { get; set; }
        public string ShortLink { get; set; }
        public string OriginalLink { get; set; }
        public int Views { get; set; }

    }
}
