# Instrucciones del proyecto

## Objetivo

Este repositorio usa una sola aplicación ASP.NET Core llamada `Api`. La arquitectura debe seguir Clean Architecture dentro del mismo proyecto, separando responsabilidades por carpetas y manteniendo dependencias dirigidas hacia adentro.

## Estructura esperada

Mantener el código organizado, como mínimo, en estas áreas:

- `Domain` para las reglas del negocio puro
- `Application` para casos de uso
- `Infrastructure` para acceso a datos y servicios externos
- `Controllers` o `Endpoints` para la capa HTTP de entrada

Ejemplo de estructura:

- `Api/Domain`
- `Api/Application`
- `Api/Infrastructure`
- `Api/Controllers`

## Reglas de Clean Architecture

- `Domain` no debe depender de `Application`, `Infrastructure` ni de detalles de infraestructura.
- `Application` puede depender de `Domain`, pero no de `Infrastructure`.
- `Infrastructure` implementa contratos definidos por `Domain` o `Application`.
- La capa HTTP solo orquesta la entrada y salida; no debe contener lógica de negocio.
- Evitar referencias circulares entre capas.
- No mover la lógica de negocio a controllers, servicios anémicos o helpers globales.

## Domain

En `Domain` deben vivir solo conceptos del negocio:

- Entidades
- Objetos de valor
- Agregados
- Reglas e invariantes del dominio
- Enumeraciones del dominio
- Eventos de dominio, si aplican
- Interfaces del dominio cuando representen contratos del negocio

Recomendaciones:

- Las entidades deben proteger sus invariantes.
- Los objetos de valor deben modelarse con Vogen, no con clases manuales.
- Las entidades solo deben declarar campos y propiedades del negocio que se reutilizan; no deben definir objetos de valor manuales dentro de la clase.
- Los objetos de valor deben ser inmutables cuando sea posible.
- No usar atributos o dependencias de EF, HTTP o infraestructura dentro del dominio.

## Application

La capa `Application` debe implementar CQRS con Vertical Slice Architecture basada en features.

Convenciones:

- Organizar por feature, no por tipo técnico.
- Cada feature debe agrupar su comando, consulta, handler, validación y contrato de respuesta.
- Separar `Commands` y `Queries` cuando ayude a la claridad.
- Mantener los casos de uso pequeños y enfocados en una sola responsabilidad.
- Preferir modelos de entrada y salida específicos para cada caso de uso.

Ejemplo de organización por feature:

- `Application/Products/CreateProduct`
- `Application/Products/GetProductById`
- `Application/Orders/PlaceOrder`

Dentro de cada feature se pueden incluir:

- Command o Query
- Handler
- Validator
- DTO o Response
- Mapeos necesarios para ese caso de uso

Reglas importantes:

- No mezclar lógica de persistencia en `Application`.
- No usar `Application` como una bolsa de servicios genéricos.
- No compartir demasiado código entre features si rompe la claridad del slice.

## CQRS

- Los comandos deben representar cambios de estado.
- Las consultas deben representar lecturas.
- Un handler debe resolver una sola intención del negocio.
- Los resultados de consulta no deben exponer entidades del dominio directamente si eso compromete el encapsulamiento.
- La forma de los contratos debe reflejar la necesidad del caso de uso, no la forma de la base de datos.

## Infrastructure

`Infrastructure` es la capa de detalles técnicos y comunicación externa.

Aquí deben ir:

- DbContext y configuraciones de persistencia
- Repositorios concretos
- Integraciones con bases de datos
- Clientes HTTP externos
- Mensajería, colas o buses
- Servicios de archivos, correo, caché u otros sistemas externos
- Implementaciones de interfaces definidas en capas internas

Reglas:

- `Infrastructure` no debe contener reglas de negocio.
- La configuración de persistencia y de integraciones debe quedarse aquí.
- Si cambia una tecnología externa, el impacto debe quedar contenido en esta capa.

## Capa HTTP

La API debe limitarse a:

- Recibir requests
- Validar lo mínimo necesario a nivel de transporte
- Mapear hacia comandos o consultas
- Delegar a `Application`
- Retornar responses HTTP

Si se usan controllers:

- Mantenerlos delgados
- No poner lógica de negocio en acciones
- No mezclar acceso a datos directo desde el controller

Si se usan endpoints minimalistas:

- Mantener cada endpoint alineado con una feature
- Evitar endpoints monolíticos o muy acoplados

## Dependencias externas y data

- Toda comunicación con bases de datos, APIs externas, colas o archivos debe pasar por `Infrastructure`.
- No acceder directamente a recursos externos desde `Domain` ni desde la capa HTTP.
- Definir interfaces en capas internas y resolverlas en `Infrastructure` mediante inyección de dependencias.

## Estilo de implementación

- Preferir nombres explícitos y orientados al negocio.
- Mantener cada feature pequeña y fácil de navegar.
- Evitar utilidades compartidas que terminen escondiendo la intención del caso de uso.
- Si un cambio afecta a varias capas, hacer el cambio mínimo necesario en cada una.
- Cuando se agregue una nueva capacidad, ubicarla primero en la feature correspondiente antes de inventar abstracciones globales.

## Regla práctica

Si dudas dónde poner una pieza:

- Si es regla del negocio, va a `Domain`
- Si es caso de uso, va a `Application`
- Si es integración técnica, va a `Infrastructure`
- Si es transporte HTTP, va a `Controllers` o `Endpoints`

## Preferencias para nuevas funcionalidades

- Crear una carpeta por feature.
- Usar nombres de casos de uso, no nombres genéricos.
- Mantener el proyecto único, sin dividirlo en múltiples proyectos salvo que se pida explícitamente.
- Priorizar claridad sobre abstracciones prematuras.
