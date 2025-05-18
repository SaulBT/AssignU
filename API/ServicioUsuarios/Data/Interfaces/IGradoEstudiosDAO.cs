using ServicioUsuarios.Entities;

public interface IGradoEstudiosDAO
{
    Task<List<grado_estudio>> obtenerTodosAsync();
}