namespace UrlShortener.Core.Exceptions;

public class ForbiddenException(string message) : Exception(message);