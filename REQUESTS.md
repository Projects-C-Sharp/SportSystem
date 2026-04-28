# Enunciado de la Prueba de desempeño C#

## Caso de uso:
Un complejo deportivo gestiona actualmente sus reservas mediante procesos manuales (agendas
físicas y hojas de cálculo), lo que ha generado múltiples problemas operativos:
- oble reserva de un mismo espacio en el mismo rango de horario.
- Usuarios con múltiples reservas en horarios que se cruzan.
- Falta de control sobre los espacios deportivos disponibles.
- Dificultad para consultar disponibilidad y estado de reservas.
- Pérdida o inconsistencia de la información.

La administración ha decidido desarrollar un sistema interno en C# que permita gestionar de
forma eficiente los usuarios, espacios deportivos, reservas.

## Objetivo:
Deberás construir un sistema en C# utilizando aplicaciones de consola o web, EF Core, LINQ,
estructuras de datos como List<> y Dictionary<TKey, TValue>, y una base de datos (MySQL o
PostgreSQL).

### El sistema deberá:
- Centralizar la información de usuarios, espacios deportivos y reservas.
- Evitar conflictos de horarios mediante validaciones de negocio.
- Gestionar el ciclo completo de una reserva (creación, finalización).
- Aplicar correctamente los principios de Programación Orientada a Objetos (POO).
- Incorporar validaciones y manejo de errores para garantizar la integridad de los datos.
- (Opcional) Implementar el envío de correos electrónicos mediante SMTP.

### Funcionalidades principales:
**Para alcanzar un resultado óptimo, deberás cumplir con los siguientes requisitos:**
1. Gestión de Usuarios
   - Registrar nuevos usuarios con los siguientes datos: nombre, documento de identidad, teléfono y correo electrónico.
   - Editar la información de un usuario existente.
   - Validar que no existan usuarios duplicados mediante el documento de identidad y correo electronico.
   - Listar todos los usuarios registrados en el sistema.

2. Gestión de Espacios Deportivos
   • Registrar espacios deportivos con los siguientes datos: nombre, tipo de espacio (fútbol,
   baloncesto, piscina, etc.), capacidad.
   - Editar la información de los espacios deportivos.
   - Validar que no existan espacios duplicados.
   - Listar todos los espacios deportivos registrados.
   - Permitir filtrar espacios por tipo.
   
3. Gestión de Reservas
   - Crear reservas asociando un usuario, un espacio deportivo, una fecha, una hora de inicio y una hora de fin.
   - Validar que un espacio deportivo no tenga reservas en rangos de tiempo solapados.
   - Validar que un usuario no tenga más de una reserva en el mismo rango de horario.
   - Validar que la hora de fin sea mayor a la hora de inicio.
   - Validar que no se puedan crear reservas en fechas u horas pasadas.
   - Gestionar los estados de la reserva: Cancelada, Finalizada etc.
   - Permitir cancelar una reserva cambiando su estado a 'Cancelada'.
   - Listar reservas por usuario.
   - Listar reservas por espacio deportivo.
   
4. Notificaciones por correo.
   - Enviar un correo electrónico al usuario cuando se crea una reserva.
   
5. Persistencia de Datos
   - Utilizar estructuras de datos como List<> y Dictionary<TKey, TValue> para la gestión de la información.
   - Aplicar consultas mediante LINQ.
   - Persistir la información utilizando EF Core.
   
6. Manejo de Errores y Validaciones
   - Implementar manejo de excepciones mediante bloques try-catch.
   - Mostrar mensajes de error claros y amigables al usuario.
   - Garantizar que todas las reglas de negocio se cumplan en cada operación del sistema.

---

## Criterios de aceptación:
El sistema será considerado aprobado si cumple con los siguientes criterios:
1. Gestión de Usuarios
   - Es posible registrar un usuario con todos los datos obligatorios.
   - Es posible editar la información de un usuario.
   - El sistema valida que el documento sea único.
   - Se puede visualizar un listado completo de usuarios.
   
2. Gestión de Espacios Deportivos
   - Es posible registrar un espacio deportivo correctamente.
   - Es posible editar su información.
   - El sistema valida que no existan duplicados.
   - Se puede listar y filtrar espacios.
   
3. Gestión de Reservas
   - Es posible crear reservas válidas.
   - El sistema impide reservas en conflicto de horarios.
   - Los estados de reserva funcionan correctamente.
   - Se pueden listar reservas por usuario y por espacio.
   
4. Correos
   - Se envían correos correctamente.
   
5. Manejo de errores
   - El sistema maneja correctamente excepciones.
   - Se muestran mensajes claros.
   - Se respetan todas las reglas de negocio.


## Entregables:
- Enlace al repositorio en GitHub (público).
- Proyecto comprimido (.zip).
- Diagrama de clases.
- Diagrama de casos de uso.
- Archivo README con instrucciones detalladas para ejecutar el proyecto