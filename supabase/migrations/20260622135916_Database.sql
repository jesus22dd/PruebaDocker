-- Eliminar tablas si existen (en orden por FK)
DROP TABLE IF EXISTS public."DetalleVenta";
DROP TABLE IF EXISTS public."Venta";
DROP TABLE IF EXISTS public."Producto";
DROP TABLE IF EXISTS public."Categoria";
DROP TABLE IF EXISTS public."Cliente";
DROP TABLE IF EXISTS public."Usuario";


CREATE TABLE public."Usuario" (
    "Id"     INT GENERATED ALWAYS AS IDENTITY NOT NULL,
    "Nombre" VARCHAR(100) NOT NULL,
    "Correo" VARCHAR(100) NOT NULL,
    "Clave"  VARCHAR(255) NOT NULL,
    "Rol" VARCHAR(20) NOT NULL DEFAULT 'Worker' CHECK ("Rol" IN ('Admin', 'Worker')),
    CONSTRAINT PK_Usuario        PRIMARY KEY ("Id"),
    CONSTRAINT UQ_Usuario_Correo UNIQUE ("Correo")
);

CREATE TABLE public."Categoria" (
    "Id"     INT GENERATED ALWAYS AS IDENTITY NOT NULL,
    "Nombre" VARCHAR(100) NOT NULL,
    "HashId" VARCHAR(250) NOT NULL,
    CONSTRAINT PK_Categoria PRIMARY KEY ("Id")
);

CREATE TABLE public."Producto" (
    "Id"          INT GENERATED ALWAYS AS IDENTITY NOT NULL,
    "Nombre"      VARCHAR(100)   NOT NULL,
    "Detalle"     VARCHAR(100)   NULL,
    "Precio"      DECIMAL(18,2)  NOT NULL,
    "Stock"       INT            NOT NULL DEFAULT 0,
    "IdCategoria" INT            NOT NULL,
    "HashId"      VARCHAR(250)   NOT NULL,
    CONSTRAINT PK_Producto           PRIMARY KEY ("Id"),
    CONSTRAINT FK_Producto_Categoria FOREIGN KEY ("IdCategoria") REFERENCES public."Categoria"("Id")
);

CREATE TABLE public."Cliente" (
    "Id"       INT GENERATED ALWAYS AS IDENTITY NOT NULL,
    "HashId"   VARCHAR(250) NOT NULL,
    "Nombre"   VARCHAR(100) NOT NULL,
    "Telefono" VARCHAR(20)  NULL,
    "Correo"   VARCHAR(100) NULL,
    CONSTRAINT PK_Cliente PRIMARY KEY ("Id")
);

CREATE TABLE public."Venta" (
    "Id"        INT GENERATED ALWAYS AS IDENTITY NOT NULL,
    "HashId"    VARCHAR(250)  NOT NULL,
    "Fecha"     TIMESTAMP     NOT NULL DEFAULT NOW(),
    "Total"     DECIMAL(18,2) NOT NULL,
    "IdCliente" INT           NULL,
    CONSTRAINT PK_Venta         PRIMARY KEY ("Id"),
    CONSTRAINT FK_Venta_Cliente FOREIGN KEY ("IdCliente") REFERENCES public."Cliente"("Id")
);

CREATE TABLE public."DetalleVenta" (
    "Id"             INT GENERATED ALWAYS AS IDENTITY NOT NULL,
    "HashId"         VARCHAR(250)  NOT NULL,
    "IdVenta"        INT           NOT NULL,
    "IdProducto"     INT           NOT NULL,
    "Cantidad"       INT           NOT NULL,
    "PrecioUnitario" DECIMAL(18,2) NOT NULL,
    "SubTotal"       DECIMAL(18,2) NOT NULL,
    CONSTRAINT PK_DetalleVenta           PRIMARY KEY ("Id"),
    CONSTRAINT FK_DetalleVenta_Venta     FOREIGN KEY ("IdVenta")    REFERENCES public."Venta"("Id")    ON DELETE CASCADE,
    CONSTRAINT FK_DetalleVenta_Producto  FOREIGN KEY ("IdProducto") REFERENCES public."Producto"("Id")
);

INSERT INTO public."Usuario" ("Nombre", "Correo", "Clave","Rol")
VALUES (
    'Jesus Pablo Paz Uribe',
    'pazjesu17@gmail.com',
    '$2a$11$QJ5sM2mjgi98mDACp2Cun.IUTy/zCsPrgwFxH4liZG3UpSW0lvjBu',
    'Admin'
),(
    'Trabajador de Prueba',
    'trabajador@gmail.com',
    '$2a$11$QJ5sM2mjgi98mDACp2Cun.IUTy/zCsPrgwFxH4liZG3UpSW0lvjBu',
    'Worker'
);

INSERT INTO public."Categoria" ("Nombre", "HashId") VALUES
('Electronica', '037861cf0c0a9080c6cd8ae78eb28d4a01043aedd5f0d80c0943df58daa928b0'),
('Ropa y Calzado', '5dff16b24df03483d18cc696a3b8e4f9198c8aa0f1e5ef4841786c9f2f76aafe'),
('Hogar', '0291ac277e89adfc40b154abb787ed3a24cee3bbdd7c24b9b200aa72857a3b58'),
('Alimentos', 'facb2fb92ddef270b124d0055cd694a17b16c97cae64110548108cc8c219823b'),
('Ferreteria', '1a384504af315477508fc7aafaebd5f97244e3eed26b5121cca6b713de75860d');

INSERT INTO public."Producto" ("Nombre", "Detalle", "Precio", "Stock", "IdCategoria", "HashId") VALUES
('Smartphone Galaxy S23', '128GB, Color Negro', 850.00, 100, 1, 'b4e35737c0e62215662d7a52b2325d772819e1f0fbeef8134a270a6f48bde989'),
('Laptop Dell XPS 15', 'Core i7, 16GB RAM', 1500.00, 50,  1, 'a2b90edec0e6f7f488fe594bd79a8fe1b4c0eccffc3aeeea73a0d4088917ef7d'),
('Auriculares Sony WH-1000XM5', 'Inalambricos, Cancelacion ruido', 350.00, 30,  1, '12f9b84cf2f5ff244ccfc9c9d6543b0c760ebd40af65d695545a6f64b72b1242'),
('Camiseta de Algodon', 'Talla M, Color Blanco', 15.50, 200, 2, 'e25c4fe2d3ede2a25450d6159d5fc0554fcf030d17572c7ec18511c5f72df6cd'),
('Pantalon Jean Levis', 'Corte Recto, Talla 32', 45.00, 150, 2, '7e0e0e1e91feaadf9d36d09ea061db4be3551fd72e0a944e9683d98a5a5a72f0'),
('Chaqueta de Cuero', 'Estilo Biker, Color Cafe', 120.00, 60,  2, '8ebc71154bd8cd6670d7cd59840950459dfa16080141dc8801022dd0d04ffd0c'),
('Licuadora Oster', '6 Velocidades, Vaso Vidrio', 65.00, 100, 3, 'ad40bacf6ebfd9deaf78ff0b830537871cd0ae664e1d778763455951c9397bfa'),
('Juego de Sabanas King', 'Algodon Egipcio, 400 hilos', 85.50, 50,  3, '1e49617bd7ebb99f804eaf93ea62f0c6262d19be8fda1d03bf1f47bbdeba9bbe'),
('Sofa de 3 Plazas', 'Tapizado en tela gris', 450.00, 40,  3, '544e764773a6d5c559396c7cac993ad0f2dae1395551cb11b3c478b4fa5e3e1d'),
('Arroz Grano Largo', 'Bolsa de 5 kg', 12.00, 250, 4, '088fbe3bcfb6c413ca0d83f46f6d75a1f87b343625ee15e1415a403b04ac1f77'),
('Aceite de Oliva Extra Virgen', 'Botella 1 Litro', 18.50, 350, 4, '57444be76c53dc6f54af491aa4de6a8b8b88c89d83aa5225568d40a7d7172305'),
('Cafe Tostado y Molido', 'Paquete 500g, Origen Colombia', 9.00, 800, 4, '545a5e56a97d896df04fc0fe88cad445df1eff9a07528ebcf18048fe108df7f5'),
('Taladro Inalambrico Dewalt', 'Bateria 20V, Incluye cargador', 140.00, 100, 5, 'cc5396450f5eecefa0379574a599c8170754ddf9bc48b042aa9a235a9e973d24'),
('Juego de Destornilladores', 'Kit de 6 piezas magneticas', 25.50, 120, 5, '67986c344d99dae6d1eab64f73e2bc7bc59a656f49d10d45cb60dae57a5c6ec5'),
('Martillo de Acero', 'Mango de goma ergonomico', 15.00, 180, 5, 'e9a39590d9aaecc368b7354d47c2bc990b9cef26e9a1e929d13ce1e8f35c100e');

INSERT INTO public."Cliente" ("HashId", "Nombre", "Telefono", "Correo") VALUES
('f79004dcfff88b5fd1606b0e6da67aa62b7b540f0a8023caabd241c45533cb77', 'Carlos Mendoza', '70000001', 'cmendoza@gmail.com'),
('2b3c148fb0b56e8c0014aa219929f3474e6143393ee570bac7331e866a5a8ef9', 'Ana Torres', '70000002', 'atorres@hotmail.com'),
('df529acd42a108239be0e1a92ba990c047a8d36e77cafe3569207fc0f7438384', 'Luis Fernando Gomez', '70000003', 'lfgomez@outlook.com'),
('e88b3da03b98c0348b14cf5a20ec1870a3ba3035af8b4aedacdcd45775686e23', 'Maria Rene Silva', '70000004', 'msilva@gmail.com'),
('49d98be85fd9c84f29491b93594173601866536a7cbb1803f1d02f6f899d51ac', 'Jorge Vargas', '70000005', 'jvargas@yahoo.com'),
('4d88089baeab096eda29d38b6fb1c1c511daa1db0003f6e6f5aa5dad289e1eca', 'Camila Rojas', '70000006', 'crojas@gmail.com'),
('ea5b11a623913407a15b90ad676b1a47f287a3d93aff541c8b8eb9f1e03e1827', 'Pedro Castillo', '70000007', 'pcastillo@hotmail.com'),
('cadde132c4e3608ba4703d095aa33813345c3309f70c7abd745d1800001b767c', 'Sofia Gutierrez', '70000008', 'sgutierrez@outlook.com'),
('a044789f76ed595891556bb76881aba56e066a4f5cfec29de987f7dcc00662b5', 'Diego Morales', '70000009', 'dmorales@gmail.com'),
('fdc8830d7b36ccb28dd3ef26522fde0e890d6ad16f8ec1602ff5799313686644', 'Laura Fernandez', '70000010', 'lfernandez@yahoo.com'),
('4f634010e1efd4edddec8c1833da31750c8918d86191206da42f93799720020c', 'Andres Suarez', '70000011', 'asuarez@gmail.com'),
('fbbdb0ed3321b5f25793690355cf053f64f85f0cc767ebe22bfa7f4f1e0b2242', 'Valentina Castro', '70000012', 'vcastro@hotmail.com'),
('daf0b34bd3177af41e92b3121160a76213f7364056642ddffe05f03580bbf768', 'Javier Ortiz', '70000013', 'jortiz@gmail.com'),
('4a5490463361e05bffe556a50c034d17f238195d1abc54cde30848080d6d7a06', 'Luciana Herrera', '70000014', 'lherrera@outlook.com'),
('36e86c90a73c746ee277cfd49df179860e3a320aca122dbeadbe085ce9c322cf', 'Roberto Paredes', '70000015', 'rparedes@gmail.com');

-- Datos de prueba para Venta
INSERT INTO public."Venta" ("HashId", "Fecha", "Total", "IdCliente") VALUES
('1a2b3c4d5e6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b', '2026-06-25 09:15:00', 850.00, 1),
('2b3c4d5e6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c', '2026-06-25 10:30:00', 1515.50, 2),
('3c4d5e6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d', '2026-06-25 14:45:00', 395.00, 3),
('4d5e6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e', '2026-06-26 11:20:00', 90.00, 4),
('5e6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f', '2026-06-26 15:10:00', 145.50, 5),
('6f7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a', '2026-06-27 09:05:00', 240.00, 6),
('7a8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b', '2026-06-27 12:40:00', 180.00, 7),
('8b9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b9c', '2026-06-28 10:55:00', 535.50, 8),
('9c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b9c0d', '2026-06-28 16:15:00', 36.00, 9),
('0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b9c0d1e', '2026-06-29 09:30:00', 55.50, 10),
('1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b9c0d1e2f', '2026-06-29 11:45:00', 850.00, 11),
('2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b9c0d1e2f3a', '2026-06-29 14:20:00', 450.00, 12),
('3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b9c0d1e2f3a4b', '2026-06-30 10:10:00', 18.50, 13),
('4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b9c0d1e2f3a4b5c', '2026-06-30 12:30:00', 140.00, 14),
('5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b9c0d1e2f3a4b5c6d', '2026-06-30 15:05:00', 55.50, 15);

-- Datos de prueba para DetalleVenta
INSERT INTO public."DetalleVenta" ("HashId", "IdVenta", "IdProducto", "Cantidad", "PrecioUnitario", "SubTotal") VALUES
('1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a', 1, 1, 1, 850.00, 850.00),
('2b2b2b2b2b2b2b2b2b2b2b2b2b2b2b2b2b2b2b2b2b2b2b2b2b2b2b2b2b2b2b2b', 2, 2, 1, 1500.00, 1500.00),
('3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c3c', 2, 4, 1, 15.50, 15.50),
('4d4d4d4d4d4d4d4d4d4d4d4d4d4d4d4d4d4d4d4d4d4d4d4d4d4d4d4d4d4d4d4d', 3, 3, 1, 350.00, 350.00),
('5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e5e', 3, 5, 1, 45.00, 45.00),
('6f6f6f6f6f6f6f6f6f6f6f6f6f6f6f6f6f6f6f6f6f6f6f6f6f6f6f6f6f6f6f6f', 4, 5, 2, 45.00, 90.00),
('7a7a7a7a7a7a7a7a7a7a7a7a7a7a7a7a7a7a7a7a7a7a7a7a7a7a7a7a7a7a7a7a', 5, 7, 1, 65.00, 65.00),
('8b8b8b8b8b8b8b8b8b8b8b8b8b8b8b8b8b8b8b8b8b8b8b8b8b8b8b8b8b8b8b8b', 5, 8, 1, 85.50, 85.50),
('9c9c9c9c9c9c9c9c9c9c9c9c9c9c9c9c9c9c9c9c9c9c9c9c9c9c9c9c9c9c9c9c', 6, 6, 2, 120.00, 240.00),
('0d0d0d0d0d0d0d0d0d0d0d0d0d0d0d0d0d0d0d0d0d0d0d0d0d0d0d0d0d0d0d0d', 7, 10, 15, 12.00, 180.00),
('1e1e1e1e1e1e1e1e1e1e1e1e1e1e1e1e1e1e1e1e1e1e1e1e1e1e1e1e1e1e1e1e', 8, 9, 1, 450.00, 450.00),
('2f2f2f2f2f2f2f2f2f2f2f2f2f2f2f2f2f2f2f2f2f2f2f2f2f2f2f2f2f2f2f2f', 8, 8, 1, 85.50, 85.50),
('3a3a3a3a3a3a3a3a3a3a3a3a3a3a3a3a3a3a3a3a3a3a3a3a3a3a3a3a3a3a3a3a', 9, 10, 3, 12.00, 36.00),
('4b4b4b4b4b4b4b4b4b4b4b4b4b4b4b4b4b4b4b4b4b4b4b4b4b4b4b4b4b4b4b4b', 10, 11, 3, 18.50, 55.50),
('5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c5c', 11, 1, 1, 850.00, 850.00),
('6d6d6d6d6d6d6d6d6d6d6d6d6d6d6d6d6d6d6d6d6d6d6d6d6d6d6d6d6d6d6d6d', 12, 9, 1, 450.00, 450.00),
('7e7e7e7e7e7e7e7e7e7e7e7e7e7e7e7e7e7e7e7e7e7e7e7e7e7e7e7e7e7e7e7e', 13, 11, 1, 18.50, 18.50),
('8f8f8f8f8f8f8f8f8f8f8f8f8f8f8f8f8f8f8f8f8f8f8f8f8f8f8f8f8f8f8f8f', 14, 13, 1, 140.00, 140.00),
('9a9a9a9a9a9a9a9a9a9a9a9a9a9a9a9a9a9a9a9a9a9a9a9a9a9a9a9a9a9a9a9a', 15, 14, 1, 25.50, 25.50),
('0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b', 15, 15, 2, 15.00, 30.00);
