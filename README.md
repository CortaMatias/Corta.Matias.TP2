## **TRUCO.NET**

 SOBRE MI : 
 
 
### Mi nombre es **Matias Corta**

*Este trabajo fue mi mayor desafio desde que estoy programando, me gusto mucho hacerlo porque me enseño un monton de cosas. Estoy contento con el proyecto que logre, el mayor desafio se me planteo a la hora de desarrollar la logica de las partidas de truco ya que son 100% simuladas pero no de manera random, sino que tiene una logica con la que se van a manejar los jugadores lo mas parecido posible a la realidad. Espero que les guste!*

<h2> Resumen de la aplicacion </h2>

<ul>
  <li><h3>Lobby</h3>  
  La aplicacion comienza con esta vista, donde se encuentran las listas de partidas jugadas y finalizadas y donde estan estos 3 botones.
  
  Las listas de partidas estara con los nombres de los Jugadores que esten dentro de la sala jugando una partida.
  Al hacerle doble click al boton se abrira "La sala".
  
 ![](https://github.com/CortaMatias/Corta.Matias.TP2/blob/main/Fotos%20Readme/Lobby.jpeg)</li>
  
  <li><h3>Crear Sala</h3>
  - Al hacer click en el boton "Crear Sala" entraremos en esta vista que nos permitira seleccionar los usuarios en un comboBox que seran traidos desde la base de datos. Debemos elegir 2 Usuarios y comenzar y crear la sala. Una vez creada la sala veremos en la lista del lobby agregada la sala. Y podremos acceder a ella haciendole doble click en el item del ListBox;
  
  ![](https://github.com/CortaMatias/Corta.Matias.TP2/blob/main/Fotos%20Readme/CrearSala.jpeg) </li>
  
  
  <li><h3>Sala</h3>
  - Al hacer doble click sobre las listas de partidas se abrira esta vista que nos permite ver lo que esta pasando en la partida, o lo que ya paso si termino.
  - Si la partida esta en curso tendremos la opcion de finalizar la partida de manera manual. Esto hara que la sala salga de la lista de partidas que se esten jugando y se vaya a la lista de partidas finalizadas, sin embargo estas partidas no seran guardadas en la base de datos ya que no finalizaron de manera "Normal".
  - Tambien tendremos la opcion ya sea que se este jugando o no la partida de apretar el Boton "Descargar Logs" que nos permitira descargar el resumen de la partida en un archivo txt que estara dentro de una carpeta en el escritorio.
   
 ![](https://github.com/CortaMatias/Corta.Matias.TP2/blob/main/Fotos%20Readme/Sala.jpeg) </li> 
  
  <li><h3>Jugadores</h3>
  - Al apretar el boton Jugadores del Lobby entraremos a esta vista donde estara la lista de los Usuarios junto con sus estadisticas sobre las partidas jugadas.
  - Vamos a poder agregar nuevos Jugadores, modificar sus datos, y eliminarlos.
  - Una vez finalizada una partida se actualizaran los datos de los jugadores sumando o restando la cantidad de victorias y derrotas.
  
 ![](https://github.com/CortaMatias/Corta.Matias.TP2/blob/main/Fotos%20Readme/Jugadores.jpeg) </li>
  
  <li><h3>Partidas</h3>
  - Al apretar el boton Partidas del Lobby entramos a esta vista donde estara la lista de Partidas jugadas con sus respectivos Jugadores ganador y perdedor, el puntaje de la partida,el id de dicha partida, la fecha en la que se jugo y la cantidad de manos que llevo que finalice la partida. 
  
  ![](https://github.com/CortaMatias/Corta.Matias.TP2/blob/main/Fotos%20Readme/Partida.jpeg) </li>
</ul>
 
 
 <h4> Justificacion tecnica</h4>
 
 - Se logro implementar todos los temas requeridos.
 - SQL -> Lo utilice para persistir los datos de las partidas jugadas y tambien los datos de los jugadores asi como tambien para actualizar dichos datos.
 - Manejo de excepciones -> Lo utilice para manejar lo que serian las interacciones con archivos y base de datos o errores generados por el usuario a la hora de manejar la aplicacion.
 - Unit testing -> Lo utilice para chequear el correcto funcionamiento invididual de los metodos junto avanzaba con el proyecto lo cual me vino muy bien para poner a prueba los metodos ante diferentes circunstancias como por ejemplo para calcular el envido, manejar el ranking de cartas, etc.
 - Generics -> Lo utilice para la clase static de serializacion JSON con el objetivo de poder Serializar o Deserializar cualquier tipo de objeto, donde T sea una clase con un constructor public(). Tambien lo utilice para la interfaz IDataAcess que define los metodos que van a implementar las clases que necesiten conectarse con la base de datos.
 - Serializacion -> Lo utilice a persistir el mazo en un archivo y levantarlo cuando sea necesario, esta la clase SerializadorJSON que es generica que permite Serializar o deserializar cualquier tipo de objeto.
 - Escritura de archivos -> Lo utilice para guardar el resumen de la partida en un archivo.txt y para guardar el mazo serializado en un archivo .Json 
 - Interfaces -> Lo implemente a traves de la IDataAcess que define los metodos que van a implementar las clases que necesiten conectarse con la base de datos, esta interfaz es generica lo que permite que cualquier clase nueva que necesite implementar estas definiciones solo debe utilizar esta interfaz.
 - Delegados -> Lo utilice para pasarle la lista de metodos a ejecutar para cada evento determinado.
 - Task -> Utilize la task para crear un hilo cada vez que se instancie una nueva sala, este hilo ejecutara el metodo ComenzarJuego() que comenza a simular las partidas y se cancelara cuando termine la partida o cuando se active el token de cancelacion. 
 - Eventos -> Los utilice para notificar las cosas que van ocurriendo dentro de las instancias de la clase Sala, como por ejemplo para actualizar el RichTextBox de el formulario sala cada vez que ocurra una accion en la partida, tambien para avisar cuando esta partida finalizo para poder actualizar los datos de los jugadores que jugaron dicha partida y para poder guardar los datos de dicha partida en la base de datos. Y una ultima implementacion que fue para ir moviendo las salas de los ListBox dependiendo si estaba finalizada o en juego.
 
 
