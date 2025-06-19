SELECT
  Titulo,
  COUNT(*) AS VecesRepetido
FROM dbo.Mangas
GROUP BY Titulo
HAVING COUNT(*) > 1;

select * from Mangas
