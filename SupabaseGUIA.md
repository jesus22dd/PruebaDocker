Instalamos supabase en el repositorio actual conectado con:
npm install -g supabase

Luego inciamos supabase en el proyecto para crear la carpeta raiz:
supabase init

Creamos la migracion, el archivo SQL para mantener el rastro de cambios directamente en la NUBE.
supabase migration new [nombre_de_la_migracion]

Luego conectamos el repositorio de supabase con la nube:
supabase login

Nos dirijimos a la pagina de supabase > Perfil > Account > Access Tokens > Generate new token > Copiamos el token y lo pegamos en el campo de seguido luego de ingresar "supabase login"

Luego finalmente conectamos y hacemos referencia de la carpeta al proyecto:
supabase link --project-ref [ID_DEL_PROYECTO]
    El ID del proyecto viene a ser la serie de caracteres seguido del "." luego del nombre de usuario que nos dio el [ConnectionString] tal cual [postgres.ID_DEL_PROYECTO]

Luego de cualquier comando en el archivo de [nombre_de_la_migracion] para ejecutar los cambios en la nube, basta con un:
supabase db push

Para que el Push funcione nuevamente luego de haber ejecutado al menos UNA VEZ, se debera limpiar el historial para poder ejecutar nuevamente el mismo archivo:
supabase migration repair --status reverted [NUMERO_MIGRACION]

Y para ver el numero de la migracion se debe ingresar el siguiente comando o directete copiar el numero que esta en el nombre de la migracion:
supabase migration list

Supabase > Connect (en la barra superior) > Direct [ConnectionString] > Select [Session_pooler] > Copiamos el [host] [puerto] [database] [user]

Luego en AppSettings.json buscamos la llave de conexion: y usamos implementamos de la siguiente manera:

 "ConnectionStrings": {
   "ConnectionSQL": "Host=[host];Port=[puerto];Database=[database];Username=[user];Password=[Contraseña_que_utilizamos_para_crear_el_proyecto_en_supabase]"
 },

NOTA: Se debe instalar previamente el nuget necesario, y rehacer un mapeo luego de crear la DB con un comando especifico desde la consola de nugets. Hacer la referencia correcta a postgress en program.cs
