SELECT 
    m.Id,
    m.Titulo,
    m.Autor,
    m.FechaPublicacion,
    m.UrlImagen,
    g.Nombre AS Genero
FROM Mangas m
LEFT JOIN Generos g ON m.GeneroId = g.GeneroId
ORDER BY m.Id;