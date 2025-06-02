using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServicioUsuarios.DAOs;
using ServicioUsuarios.DTOs;
using ServicioUsuarios.Entities;
using ServicioUsuarios.Exceptions;
using ServicioUsuarios.Services.Interfaces;

namespace ServicioUsuarios.Services.Implementation;

public class DocenteService : IDocenteService
{
    private readonly DocenteDAO _docenteDAO;

    public DocenteService(DocenteDAO docenteDAO)
    {
        _docenteDAO = docenteDAO;
    }

    public async Task<DocenteDTO> obtenerPorIdAsync(int id)
    {
        verificarIdValida(id);
        var docente = await _docenteDAO.obtenerPorIdDtoAsync(id);
        verificarExistenciaDocente(docente);
        return docente;
    }

    public async Task<docente> registrarAsync(RegistrarDocenteDTO docenteDto)
    {
        verificarDocenteNuloRegistro(docenteDto);
        verificarDocenteNombreRegistroAsync(docenteDto.nombreUsuario);
        verificarDocenteCorreoAsync(docenteDto.correoElectronico);

        var nuevoDocente = new docente
        {
            nombreCompleto = docenteDto.nombreCompleto,
            nombreUsuario = docenteDto.nombreUsuario,
            contrasenia = docenteDto.contrasenia,
            correo = docenteDto.correoElectronico,
            idGradoProfesional = docenteDto.idGradoProfesional
        };

        await _docenteDAO.agregarDocenteAsync(nuevoDocente);

        return nuevoDocente;
    }

    public async Task actualizarAsync(int id, ActualizarDocenteDTO docenteDto)
    {
        verificarIgualdadId(id, docenteDto.idDocente);
        verificarDocenteNuloActualizacion(docenteDto);
        verificarDocenteNombreActualizacionAsync(docenteDto.nombreUsuario, id);

        var docente = await _docenteDAO.obtenerPorIdNormalAsync(id);
        docente.nombreCompleto = docenteDto.nombreCompleto;
        docente.nombreUsuario = docenteDto.nombreUsuario;
        docente.idGradoProfesional = docenteDto.idGradoProfesional;

        await _docenteDAO.actualizarAsync(docente);
    }

    public async Task eliminarAsync(int id)
    {
        verificarIdValida(id);
        await _docenteDAO.eliminarAsync(id);
    }

    private void verificarIdValida(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El ID del docente debe ser mayor que cero.", nameof(id));
        }
    }

    private void verificarExistenciaDocente(DocenteDTO docente)
    {
        if (docente == null)
        {
            throw new KeyNotFoundException("El docente no existe.");
        }
    }

    private void verificarDocenteNuloRegistro(RegistrarDocenteDTO docenteDto)
    {
        if (docenteDto == null)
        {
            throw new ArgumentNullException(nameof(docenteDto), "No se envió ningún docente para registrar.");
        }
    }

    private void verificarDocenteNuloActualizacion(ActualizarDocenteDTO docenteDto)
    {
        if (docenteDto == null)
        {
            throw new ArgumentNullException(nameof(docenteDto), "No se envió ningún docente para actualizar.");
        }
    }

    private async void verificarDocenteNombreRegistroAsync(string nombreUsuario)
    {
        var docenteExistente = await _docenteDAO.obtenerPorNombreUsuarioAsync(nombreUsuario);
        if (docenteExistente != null)
        {
            throw new RecursoYaExistenteException($"El nombre de usuario '{nombreUsuario}' ya está en uso por otro docente.");
        }
    }

    private async void verificarDocenteNombreActualizacionAsync(string nombreUsuario, int id)
    {
        var docenteExistente = await _docenteDAO.obtenerPorNombreUsuarioEIdAsync(nombreUsuario, id);
        if (docenteExistente != null)
        {
            throw new RecursoYaExistenteException($"El nombre de usuario '{nombreUsuario}' ya está en uso por otro docente.");
        }
    }

    private void verificarIgualdadId(int id, int idDocente)
    {
        if (id != idDocente)
        {
            throw new ArgumentException("El ID del docente no coincide con el ID proporcionado.", nameof(id));
        }
    }

    public async void verificarDocenteCorreoAsync(string correo)
    {
        var docenteExistente = await _docenteDAO.obtenerPorCorreoAsync(correo);
        if (docenteExistente != null)
        {
            throw new RecursoYaExistenteException($"El correo '{correo}' ya está en uso por otro docente.");
        }
    }
}