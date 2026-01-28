# PokeApi

La PokeAPI expone sus recursos de forma pública y no requiere autenticación.  
Para listar los Pokémon, la API utiliza un sistema de paginación basado en los parámetros `limit` y `offset`, y no cuenta con un endpoint específico para realizar búsquedas por texto, por lo que esa parte debe resolverse desde el frontend.

## Paginación

Para obtener un listado de Pokémon se utiliza el endpoint de la documentacion: https://pokeapi.co/api/v2/pokemon

Este endpoint permite controlar la paginación mediante dos parámetros:

- `limit`: indica cuántos Pokémon se desean obtener por petición.
- `offset`: indica desde qué posición del listado se comienza a traer la información.

La respuesta del API incluye información adicional como:
- `count`: el total de Pokémon disponibles.
- `next`: la URL de la siguiente página.
- `previous`: la URL de la página anterior.
- `results`: una lista básica con el nombre del Pokémon y la URL para consultar su detalle.

La PokeAPI no maneja números de página directamente, por lo que en el frontend se deben calcular.  
El `offset` se obtiene multiplicando la página actual menos uno por el `limit`.

la respuesta no devuelve toda la información del Pokémon, sino una lista básica con la siguiente estructura:

Es decir, cada elemento solo incluye el nombre y una URL que apunta al detalle completo del Pokémon.  
Para mostrar información más completa (imagen, tipos, estadísticas, etc.), es necesario hacer una petición adicional por cada Pokémon usando esa URL.

## Llamadas paralelas con ForkJoin

Para resolver esto de forma eficiente, en el frontend se utiliza `forkJoin` de RxJS.  
Este operador permite ejecutar múltiples peticiones HTTP en paralelo y recibir todas las respuestas al mismo tiempo, una vez que todas hayan finalizado.

Asi fue como obtuve los sprites, parte de la información del pókemon y entre otros detalles que ayudan a que la pagina luzca mejor
