# USERS

**Get All Users**
----

* **URL:**
  `/users`

* **Method**
  `GET`

* **URL Params**
  `NONE`

* **Data Params**
  `NONE`

* **Content response:** 
  ```JSONasJS
  [
  {
  "id":1,
  "name":"nombre1",
  "lastName":"apellido1",
  "age":11,
  "neighborhood":"barrio1",
  "gender":"genero1",
  "mail":"mail1@mail.com",
  "password":"password1",
  "salt":"ksskskskaok2w3i0a2kdk2ak",
  "state":1,
  },
  "id":1,
  "name":"nombre2",
  "lastName":"apellido2",
  "age":22,
  "neighborhood":"barrio2",
  "gender":"genero2",
  "mail":"mail2@mail.com",
  "password":"password2",
  "salt":"aowawplcna3na329poas",
  "state":1,
  }
  ]
  ```

**Get One By ID**
----

* **URL**
  `/users/:id`

* **Method**
  `GET`

* **URL Params**
  `id=[integer]`

* **Data Params**
  `NONE`

* **Content response:** 
  ```JSONasJS
  {  
  "id":1,
  "name":"nombre1",
  "lastName":"apellido1",
  "age":11,
  "neighborhood":"barrio1",
  "gender":"genero1",
  "mail":"mail1@mail.com",
  "password":"password1",
  "salt":"ksskskskaok2w3i0a2kdk2ak",
  "state":1,
  }
  ```

**Create**
----

* **URL**
  `/users/create`

* **Method**
  `POST`

* **URL Params**
  `NONE`

* **Data Params**
  ```JSONasJS
  {
  "name":"nombre1",
  "lastName":"apellido1",
  "age":11,
  "neighborhood":"barrio1",
  "gender":"genero1",
  "mail":"mail1@mail.com",
  "password":"password1",
  }
  ```

* **Content response:** 
  `ID of User Created`

**Update**
----

* **URL**
`/users/update`

* **Method**
  `PUT`

* **URL Params**
  `NONE`

* **Data Params**
  ```JSONasJS
  {
  "id":21
  "name":"nombreCambiado",
  "lastName":"apellidoCambiado",
  "age":99,
  "neighborhood":"barrioCambiado",
  "gender":"Masculino",
  "mail":"mailCambiado@cambiado.com",
  "password":"password1",
  }
  ```

* **Content response:** 
  `Usuario updateado [name] [mail]`

**Delete**
----

* **URL**
`/users/delete`

* **Method**
`DELETE`

* **URL Params**
`NONE`

* **Data Params**
  `id=[integer]`

* **Content response:** 
  `Usuario borrado [id]`