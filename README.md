<h1>School Management System - Microsoft ASP.NET Core 2.1</h1>

<h2>Acerca del proyecto</h2>
Este proyecto tiene por objetivo la construcción de un servicio web tipo <strong>API RESTful</strong>. Se utilizó la versión <strong>ASP.NET Core 2.1</strong>, aplicando los patrones de diseño <strong>MVC</strong> para "endpoints", se diseñaron e implemantaron <strong>Servicios</strong>, aplicados a través de <strong>Inyección de dependencias</strong> y los patrones de diseño: <strong>Unidad de trabajo</strong> (Unit of Work) y el <strong>Patrón basado en Repositorios</strong> (Reposiroty Pattern). La mayoría de los métodos diseñados y aplicados son <strong>asíncronos</strong>.

Además, se utilizó las librería <strong>LINQ</strong> para el uso de Queryies y la librería <strong>AutoMapper</strong> para realizar mapeos de objeto a objeto, esto con el fin de poder trabajar con distintos enfoques como <strong>Objetos de Transferencia de Datos</strong> / <strong>Data Transfer Objects</strong> (DTOs) o el patrón de diseño MVVM. Para la conexión de la web API con la base de datos (desarrollada para <strong>SQL Server</strong>) se utilizó <strong>Entity Framework Core</strong>, utilizando el enfoque <strong>"Base de datos primero"</strong>, por lo que se aplicó ingeniería inversa para abstraer el modelo de la base de datos.

<h2>About this project</h2>

The porpuse of this project is to build a <strong>RESTful API</strong>, using <strong>ASP.NET Core version 2.1</strong>. This web application is based on the <strong>MVC architecture</strong>, for the API endpints, supporting and implementing <strong>async calls</strong>. Also, it's been designed and implemented <strong>services</strong> using <strong>Dependency Injection</strong>, and based on the design patterns: <strong>Unit of Work</strong> and <strong>Repository pattern</strong>.

This project uses, for the database connection and comunication, the libraries of <strong>Entity Framework</strong> Core using <strong>Database first approach</strong>, on a <strong>SQL Server</strong> database. It's been applied reverse engineer to abstract the database structure (through <strong>Entity Framework Scaffolding</strong>). In addition, this API works with <strong>LINQ</strong> library for querying and <strong>Automapper</strong> for object to object mapping, in order to use <strong>Data Transfer Objects</strong> (DTOs) or MVVM pattern.

<strong></strong>
