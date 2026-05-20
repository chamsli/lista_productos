# 🛒 lista_productos

## 1. Contexto
Es una Lista de la compras y he aniadido el Campo `cantidad`.

## 2. Base de datos
Tabla `Productos`: Id (INT PK), nombre (TEXT), comprado (INT 0/1), cantidad (INT).

## 3. URLs principales
- `/Productos` – Listar
- `/Productos/Aniadir` – Crear
- `/Productos/Editar/{id}` – Editar
- `/Productos/Comprado/{id}` – Marcar comprado
- `/Productos/Eliminar/{id}` – Eliminar
- `/Productos/Detalle/{id}` – Ver detalle (extra)
