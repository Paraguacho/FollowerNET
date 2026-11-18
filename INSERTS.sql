INSERT INTO `roles` (`IdRol`, `NombreRol`) VALUES 
(1, 'ResponsablePrograma'),
(2, 'Egresado');

INSERT INTO `usuarios` (`IdUsuario`, `IdRol`, `Correo`, `ContrasenaHash`) 
VALUES (1, 1, 'coordinador.mied@itson.edu.mx','contrasena'),
(2,2,'egresado@gmail.com','contrasena');

-- Insertar el Programa Educativo 
INSERT INTO `programas_educativos` (`IdPrograma`, `Siglas`, `NombrePrograma`) 
VALUES (1, 'MIED', 'Maestría en Innovación Educativa');

-- Asignar un Responsable de Programa 
INSERT INTO `programas_responsables` (`IdUsuario`, `IdPrograma`, `FechaAsignacion`) 
VALUES (1, 1, now());

INSERT INTO `Egresados` (`IdEgresado`, `EstadoSolicitud`,`PrivacidadPerfil`,`AutorizaDirectorio`,`IdUsuario`, `IdPrograma`, `Nombres`, `ApellidoPaterno`, `ApellidoMaterno`,`FechaAceptacionPrivacidad`,`FechaUltimaActualizacion`) 
VALUES 
    (1,"Aprobado","Privado",0, 2, 1, 'Sebastián', 'Martínez', 'Soqui',NOW(),NOW());
    