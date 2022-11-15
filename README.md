## **TRUCO.NET**

 SOBRE MI : 
 
### Mi nombre es **Matias Corta**

*Este trabajo fue mi mayor desafio desde que estoy programando, me gusto mucho hacerlo porque me enseño un monton de cosas. Estoy contento con el proyecto que logre, el mayor desafio se me planteo a la hora de desarrollar la logica de las partidas de truco ya que son 100% simuladas pero no de manera random, sino que tiene una logica con la que se van a manejar los jugadores lo mas parecido posible a la realidad. Espero que les guste!*

<h2> Resumen de la aplicacion <h2/>

<ul>
  <li><h3>Lobby</h3>  
  La aplicacion comienza con esta vista, donde se encuentran las listas de partidas jugadas y finalizadas y donde estan estos 3 botones.
  
  Las listas de partidas estara con los nombres de los Jugadores que esten dentro de la sala jugando una partida.
  Al hacerle doble click al boton se abrira "La sala".
  ![Image text] FOTO LOBBY<li/>
  
  <li><h3>Crear Sala</h3>
  - Al hacer click en el boton "Crear Sala" entraremos en esta vista que nos permitira seleccionar los usuarios en un comboBox que seran traidos desde la base de datos. Debemos elegir 2 Usuarios y comenzar y crear la sala. Una vez creada la sala veremos en la lista del lobby agregada la sala. Y podremos acceder a ella haciendole doble click en el item del ListBox;
  
  ![Image text] FOTO CREAR SALA <li/>
  
  
  <li><h3>Sala</h3>
  - Al hacer doble click sobre las listas de partidas se abrira esta vista que nos permite ver lo que esta pasando en la partida, o lo que ya paso si termino.
  - Si la partida esta en curso tendremos la opcion de finalizar la partida de manera manual. Esto hara que la sala salga de la lista de partidas que se esten jugando y se vaya a la lista de partidas finalizadas, sin embargo estas partidas no seran guardadas en la base de datos ya que no finalizaron de manera "Normal".
  - Tambien tendremos la opcion ya sea que se este jugando o no la partida de apretar el Boton "Descargar Logs" que nos permitira descargar el resumen de la partida en un archivo txt que estara dentro de una carpeta en el escritorio.
  ![Image text] FOTO SALA <li/>  
  
  <li><h3>Jugadores</h3>
  - Al apretar el boton Jugadores del Lobby entraremos a esta vista donde estara la lista de los Usuarios junto con sus estadisticas sobre las partidas jugadas.
  - Vamos a poder agregar nuevos Jugadores, modificar sus datos, y eliminarlos.
  - Una vez finalizada una partida se actualizaran los datos de los jugadores sumando o restando la cantidad de victorias y derrotas.
  
  ![Image text] FOTO JUGADORES <li/>
  
  <li><h3>Partidas</h3>
  - Al apretar el boton Partidas del Lobby entramos a esta vista donde estara la lista de Partidas jugadas con sus respectivos Jugadores ganador y perdedor, el puntaje de la partida,el id de dicha partida, la fecha en la que se jugo y la cantidad de manos que llevo que finalice la partida. 
  
  ![Image text] <li/>
<ul/>
