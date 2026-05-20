using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using TodoMVC.Data;
using TodoMVC.Models;

namespace TodoMVC.Controllers;

public class ProductosController : Controller
{
    public IActionResult Index()
    {
        var producto = new List<Productos>();

        using var conexion = Database.AbrirConexion();
        var sql = "SELECT Id, nombre, comprado, cantidad FROM Productos ORDER BY Id DESC";
        using var comando = new SqliteCommand(sql, conexion);
        using var reader = comando.ExecuteReader();

        while (reader.Read())
        {
            producto.Add(new Productos
            {
                Id = reader.GetInt32(0),
                nombre = reader.GetString(1),
                comprado = reader.GetInt32(2) == 1,
                cantidad = reader.GetInt32(3)
            });
        }

        return View(producto);
    }

    public IActionResult Aniadir()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Aniadir(Productos producto)
    {
        if (string.IsNullOrWhiteSpace(producto.nombre) || producto.nombre.Length < 3)
        {
            ViewBag.Error = "El nombre debe tener al menos 3 caracteres";
            return View(producto);
        }

        using var conexion = Database.AbrirConexion();
        var sql = "INSERT INTO Productos (nombre, comprado, cantidad) VALUES (@nombre, @comprado, @cantidad)";
        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@nombre", producto.nombre);
        comando.Parameters.AddWithValue("@comprado", producto.comprado);
        comando.Parameters.AddWithValue("@cantidad", producto.cantidad);
        comando.ExecuteNonQuery();

        return RedirectToAction("Index");
    }

    public IActionResult Comprado(int id)
    {
        using var conexion = Database.AbrirConexion();
        var sql = "UPDATE Productos SET comprado = 1 WHERE Id = @id";
        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", id);
        comando.ExecuteNonQuery();

        return RedirectToAction("Index");
    }

    public IActionResult Eliminar(int id)
    {
        using var conexion = Database.AbrirConexion();
        var sql = "DELETE FROM Productos WHERE Id = @id";
        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", id);
        comando.ExecuteNonQuery();

        return RedirectToAction("Index");
    }



    public IActionResult Editar(int id)
    {
        using var conexion = Database.AbrirConexion();
        var sql = "SELECT Id, nombre, comprado, cantidad FROM Productos WHERE Id = @id";

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", id);

        using var reader = comando.ExecuteReader();

        if (!reader.Read()) return NotFound();

        var producto = new Productos
        {
            Id = reader.GetInt32(0),
            nombre = reader.GetString(1),
            comprado = reader.GetInt32(2) == 1,
            cantidad = reader.GetInt32(3)
        };

        return View(producto);
    }




    [HttpPost]
    public IActionResult Editar(Productos producto)
    {
        if (string.IsNullOrWhiteSpace(producto.nombre) || producto.nombre.Length < 3)
        {
            ViewBag.Error = "El nombre debe tener al menos 3 caracteres";
            return View(producto);
        }

        using var conexion = Database.AbrirConexion();

        var sql = "UPDATE Productos SET nombre = @nombre, comprado = @comprado, cantidad = @cantidad WHERE Id = @id";

        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.AddWithValue("@nombre", producto.nombre);
        comando.Parameters.AddWithValue("@comprado", producto.comprado);
        comando.Parameters.AddWithValue("@cantidad", producto.cantidad);
        comando.Parameters.AddWithValue("@id", producto.Id);

        comando.ExecuteNonQuery();

        return RedirectToAction("Index");
    }

    public IActionResult Detalle(int id)
    {
        using var conexion = Database.AbrirConexion();
        var sql = "SELECT Id, nombre, comprado, cantidad FROM Productos WHERE Id = @id";
        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", id);
        using var reader = comando.ExecuteReader();

        if (!reader.Read()) return NotFound();

        var producto = new Productos
        {
            Id = reader.GetInt32(0),
            nombre = reader.GetString(1),
            comprado = reader.GetInt32(2) == 1,
            cantidad = reader.GetInt32(3)
        };

        return View(producto);
    }

}



/*

PUNTO DE PARTIDA
────────────────
Este proyecto sigue la estructura clásica
de tres capas (Modelo / Vista / Controlador) y realiza las operaciones CRUD
básicas: listar, crear, completar y eliminar tareas.
 
Vuestra misión es entender ese código base, cambiar el contexto de la
aplicación y añadir las mejoras que se describen a continuación.
 
 

PARTE 1 · CAMBIO DE CONTEXTO  (obligatorio)


 
  A) Biblioteca personal
     Entidad: Libro  (titulo, autor, leido: bool)
 
  B) Gestor de películas / series
     Entidad: Pelicula  (titulo, genero, vista: bool)
 
  C) Lista de la compra
     Entidad: Producto  (nombre, cantidad, comprado: bool)
 
  D) Agenda de contactos
     Entidad: Contacto  (nombre, telefono, email)
 
  E) Registro de entrenamientos
     Entidad: Entreno  (fecha, tipo, duracion_minutos, completado: bool)
 
La elección es libre, pero hay que justificarla en la memoria (ver Parte 4).
 
 

PARTE 2 · MEJORAS OBLIGATORIAS

 
2.1  AÑADIR UN CAMPO EXTRA AL MODELO
     El modelo original solo tiene Id, Titulo y Completada.
     Añade al menos un campo adicional relevante para tu contexto.
     Ejemplos: fecha de creación, prioridad (alta/media/baja), categoría...
 
     Pasos que implica:
     · Actualizar la clase del modelo (Models/)
     · Modificar el CREATE TABLE en Database.cs
     · Actualizar las queries SQL en el controlador
     · Mostrar el nuevo campo en la vista Index
 
2.2  EDITAR UN REGISTRO
     La aplicación base no permite editar una entrada ya creada.
     Implementa la funcionalidad de edición:
 
     El formulario de edición debe pre-rellenar los campos con los valores
     actuales del registro.
 
2.3  VALIDACIÓN EN EL CONTROLADOR
     Antes de insertar o actualizar, comprueba que los campos obligatorios
     no están vacíos. Si la validación falla, devuelve el formulario con un
     mensaje de error visible al usuario (sin redirigir).
 
     Mínimo: validar que el campo principal (nombre, título…) no está vacío
     y que tiene más de 2 caracteres. (puedes utilizar regex en el fromulario)
 
 
PARTE 3 · MEJORA OPCIONAL (puntuación extra)

 
Implementa UNA de las siguientes mejoras adicionales:
 
  · FILTRADO: Añade un filtro en la vista Index (pendientes / completados / todos)
              pasando el filtro como parámetro en la URL (?filtro=pendientes).

  · DETALLE:    Añade una vista de detalle (GET /Entidad/Detalle/{id}) que
                muestre toda la información de un registro en una página propia.
 
 

PARTE 4 · DOCUMENTACIÓN

Genera un archivo de readme con la siguiente información:
 
  1. Contexto elegido y justificación (3-5 líneas)
  2. Diagrama o descripción de la estructura de la BD (tabla, columnas, tipos)
  3. URLs de la aplicación y qué acción realiza cada una
  4. Explicación de los cambios realizados respecto al código base
  5. Capturas de pantalla de todas la vistas funcionando y estiladas con CSS en plan guapo.
  

  */