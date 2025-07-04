
-- ==== RESETEO DE DATOS ====
DELETE FROM registro;
DELETE FROM tarea;
DELETE FROM clase;
DELETE FROM alumno;
DELETE FROM docente;

-- ==== ALUMNOS ====
INSERT INTO alumno (idAlumno, nombreCompleto, nombreUsuario, correo, contrasenia, idGradoEstudios) VALUES
(488, 'Alejandra López Escamilla', 'AleEsca00', 'le_alejandra@gmail.com', 'ihu2LK01', 4),
(57, 'Rodrigo Solórzano Pérez', 'RodrigoSP', 'zS24016367@estudiantes.uv.mx', '4anaka3', 3),
(90, 'Eliberto Cháriz', 'hoi4', 'paradox@gmail.com', 'build42', 2),
(13, 'Elena Karen Sánchez Alonso', 'mangoNyan', 'mikasa@gmail.com', 'zanahoria', 3);

-- ==== DOCENTES ====
INSERT INTO docente (idDocente, nombreCompleto, nombreUsuario, correo, contrasenia, idGradoProfesional) VALUES
(99, 'César Efraín Dolores Cárdenaz', 'efra_dc74', 'docce1974@yahoo.com', 'kciopd4', 2),
(321, 'Salomé de la Paz Torres Pérez', 'salocelotl', 'lafloryelcanto@gmail.com', 'moisal77', 2),
(10, 'Elmer Homero', 'Elmeromero', 'xxhomeroxx@uv.mx', 'cachirula', 1),
(1984, 'Francisco Javier López Escamilla', 'PepeSquad', 'escan@gmail.com', 'ramon95', 1);

-- ==== CLASES ====
INSERT INTO clase (idClase, codigo, nombre, idDocente) VALUES
(8652, 'N3L8M1', 'Francés I', 1984),
(385, 'H94LN2', 'Proyecto de Nación', 321),
(784, 'J24L94', 'Introducción a la programación', 1984),
(2301, '9785KJ', 'Bases de datos no convencionales', 10),
(200, '89E2KV', 'México Precolombino', 321);

-- ==== REGISTROS ====
INSERT INTO registro (idRegistro, idAlumno, idClase, ultimoInicio) VALUES
(456, 90, 385, '2025-04-26 12:59:23.932'),
(7761, 57, 385, '2025-04-26 12:59:23.932'),
(123, 90, 784, '2025-04-26 12:59:23.932'),
(927, 90, 200, '2025-04-26 12:59:23.932'),
(1893, 57, 200, '2025-04-26 12:59:23.932'),
(398, 57, 784, '2025-01-14 12:59:23.932');

-- ==== TAREAS ====
INSERT INTO tarea (idTarea, idClase, nombre, fechaLimite) VALUES
(784, 784, 'Introducción a la POO', '2025-07-30 12:59:49.999'),
(2025, 200, 'Examen diagnóstico', '2025-07-30 12:59:49.999'),
(74, 200, 'Mayas', '2025-07-01 12:59:59.999');
