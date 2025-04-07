using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OceanOdyssey.Infraestructure.Data;
using OceanOdyssey.Infraestructure.Models;
using OceanOdyssey.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Infraestructure.Repository.Implementations
{
    public class RepositoryPasajero : IRepositoryPasajero
    {

        private readonly OceanOdysseyContext _context;

        public RepositoryPasajero(OceanOdysseyContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Pasajero dto)
        {
            // 1. Validación básica del DTO
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Idhabitacion <= 0)
                throw new ArgumentException("ID de habitación inválido");

            // 2. Verificar existencia de la habitación
            var habitacionExiste = await _context.Habitacion
                .AsNoTracking()
                .AnyAsync(h => h.Id == dto.Idhabitacion);

            if (!habitacionExiste)
                throw new KeyNotFoundException($"No existe la habitación con ID {dto.Idhabitacion}");

            // 3. Limpiar el tracker para evitar conflictos
            _context.ChangeTracker.Clear();

            // 4. Configurar transacción
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 5. Mapear el pasajero
                var pasajero = new Pasajero
                {
                    Nombre = dto.Nombre?.Trim(),
                    Correo = dto.Correo?.Trim(),
                    Telefono = dto.Telefono?.Trim(),
                    Direccion = dto.Direccion?.Trim(),
                    Sexo = dto.Sexo,
                    FechaNacimiento = dto.FechaNacimiento,
                    Idhabitacion = dto.Idhabitacion
                };

                // 6. Validación adicional del modelo
                var validationErrors = new List<ValidationResult>();
                if (!Validator.TryValidateObject(pasajero, new ValidationContext(pasajero), validationErrors, true))
                {
                    throw new ValidationException(string.Join("\n", validationErrors.Select(e => e.ErrorMessage)));
                }

                // 7. Guardar cambios
                _context.Pasajero.Add(pasajero);
                await _context.SaveChangesAsync();

                // 8. Confirmar transacción
                await transaction.CommitAsync();

                return pasajero.Id;
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();

                // Manejo especial para errores de FK
                if (dbEx.InnerException is SqlException sqlEx && (sqlEx.Number == 547 || sqlEx.Number == 2627))
                {
                    throw new InvalidOperationException(
                        sqlEx.Number == 547
                            ? "Violación de relación de clave foránea. Verifique los datos relacionados."
                            : "Violación de clave única. El registro ya existe.");
                }

                throw new Exception("Error al guardar en base de datos", dbEx);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error inesperado al agregar pasajero", ex);
            }
            finally
            {
                // Limpieza adicional
                _context.ChangeTracker.Clear();
            }
        }


        public Task<Pasajero> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Pasajero>> ListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
