-- Eliminar tablas si existen (en orden por FK)
DROP TABLE IF EXISTS public."DetalleVenta";
DROP TABLE IF EXISTS public."Venta";
DROP TABLE IF EXISTS public."MovimientoInventario";
DROP TABLE IF EXISTS public."Producto";
DROP TABLE IF EXISTS public."Categoria";
DROP TABLE IF EXISTS public."Cliente";
DROP TABLE IF EXISTS public."Usuario";

DROP TYPE IF EXISTS public.rol_usuario;
CREATE TYPE public.rol_usuario AS ENUM ('Admin', 'Worker');

CREATE TABLE public."Usuario" (
    "Id"     INT GENERATED ALWAYS AS IDENTITY NOT NULL,
    "Nombre" VARCHAR(100) NOT NULL,
    "Correo" VARCHAR(100) NOT NULL,
    "Clave"  VARCHAR(255) NOT NULL,
    "Rol"    public.rol_usuario NOT NULL DEFAULT 'Worker',
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

INSERT INTO public."Usuario" ("Nombre", "Correo", "Clave")
VALUES (
    'Jesus Pablo Paz Uribe',
    'pazjesu17@gmail.com',
    '$2a$11$QJ5sM2mjgi98mDACp2Cun.IUTy/zCsPrgwFxH4liZG3UpSW0lvjBu'
);
