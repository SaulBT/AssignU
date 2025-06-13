namespace ServicioClases.Data.DTOs;

public class EstadisticasClaseDTO
{
    public int IdClase { get; set; } = 0;
    public List<AlumnoEstadisticasClaseDTO> Alumnos { get; set; }
    public TareaEstadisticasClaseDTO Tareas { get; set; }
}

public class AlumnoEstadisticasClaseDTO
{
    public int IdAlumno { get; set; } = 0;
    public string NombreComeplto { get; set; } = string.Empty;
    public List<ResultadoEstadisticaClaseDTO> Resultados { get; set; }
    public DateTime UltimaConexion { get; set; }
}

public class ResultadoEstadisticaClaseDTO
{
    public int IdTarea { get; set; } = 0;
    public int Calificacion { get; set; } = 0;
}

public class TareaEstadisticasClaseDTO
{
    public int IdTarea { get; set; } = 0;
    public string Nombre { get; set; } = string.Empty;
}