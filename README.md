# Librería de Impresión de Tickets

Esta librería permite generar e imprimir tickets de manera sencilla y personalizable utilizando JavaScript. Proporciona una serie de funciones y plantillas para formatear y estructurar el contenido de los tickets.

## Requisitos

- Node.js (versión X.X.X o superior)

## Instalación

1. Clona este repositorio en tu máquina local:

   ```
   git clone https://github.com/rhonalejandro/sokectPrinter.git
   ```

2. Navega hasta el directorio del proyecto:

   ```
   cd libreria-impresion-tickets
   ```

3. Instala el paquete `http-server` globalmente ejecutando el siguiente comando:

   ```
   npm install -g http-server
   ```

4. Ejecuta el siguiente comando para iniciar un servidor local:

   ```
   http-server -p 3002
   ```

5. Abre tu navegador web y accede a la dirección `http://localhost:3002`

## Uso

1. Importa la clase `ticketPrinter` y la función de plantilla deseada en tu archivo JavaScript:

   ```javascript
   import ticketPrinter from './js/ticketPrinter.js';
   ```

2. Crea un objeto con los datos necesarios para generar el ticket. Por ejemplo:

   ```javascript
   const data = {
     "data_header": {
       "lines": [
         "Nombre de la Empresa",
         "Dirección de la Empresa",
         "Teléfono: 123-456-7890",
         "Email: info@empresa.com"
       ]
     },
     "data_client": {
       "nombre": "Juan Pérez",
       "identificacion": "123456789",
       "direccion": "Calle Principal #123",
       "telefono": "555-1234",
       "email": "juan.perez@example.com"
     },
     "data_invoice": {
       "numero_factura": "F001",
       "fecha_emision": "2023-06-15",
       "metodo_pago": "Efectivo",
       "subtotal": 100.00,
       "impuesto": 15.00,
       "total": 115.00,
       "pagado": 120.00,
       "vuelto": 5.00
     },
     "data_invoice_items": [
       {
         "descripcion": "Producto 1",
         "cantidad": 2,
         "precio_unitario": 25.00,
         "subtotal": 50.00
       },
       {
         "descripcion": "Producto 2",
         "cantidad": 1,
         "precio_unitario": 50.00,
         "subtotal": 50.00
       }
     ],
     "data_footer": {
       "lines": [
         "Gracias por su compra",
         "Lo esperamos en su próxima visita",
         "Horario de atención: Lunes a Viernes de 9:00 a 18:00",
         "Síguenos en nuestras redes sociales"
       ]
     }
   };
   ```

3. Llama a la función `printData` pasando los datos y el nombre del template:

   ```javascript
   await ticketPrinter.printData(data, 'template1');
   ```

4. Crea dentro de la carpeta `templates` tu template del ticket:

   ```javascript
   import PrintHelpers from '../src/printHelpers.js';

   const template1 = (data) => {
     let ticket = '\n\n\n';

     // Encabezado
     data.data_header.lines.forEach(line => {
       ticket += PrintHelpers.PrintCenter([line]) + '\n';
     });

     // Separador
     ticket += PrintHelpers.Separator('-') + '\n';

     // Datos del cliente
     ticket += PrintHelpers.PrintLeft([`Nombre: ${data.data_client.nombre}`]);
     ticket += PrintHelpers.PrintLeft([`Identificación: ${data.data_client.identificacion}`]);
     ticket += PrintHelpers.PrintLeft([`Dirección: ${data.data_client.direccion}`]);
     ticket += PrintHelpers.PrintLeft([`Teléfono: ${data.data_client.telefono}`]);
     ticket += PrintHelpers.PrintLeft([`Email: ${data.data_client.email}`]);

     // Separador
     ticket += PrintHelpers.Separator('-') + '\n';

     // Detalles de la factura
     ticket += PrintHelpers.PrintLeft([`Número de factura: ${data.data_invoice.numero_factura}`]);
     ticket += PrintHelpers.PrintLeft([`Fecha de emisión: ${data.data_invoice.fecha_emision}`]);
     ticket += PrintHelpers.PrintLeft([`Método de pago: ${data.data_invoice.metodo_pago}`]);

     // Separador
     ticket += PrintHelpers.Separator('-') + '\n';

     // Líneas de detalle
     const detalleData = data.data_invoice_items.map(item => [
       item.descripcion,
       `${item.cantidad}`,
       item.precio_unitario.toFixed(2),
       item.subtotal.toFixed(2)
     ]);

     ticket += PrintHelpers.PrintColumn(
       ['Descripción', 'Cant.', 'Precio', 'Subtotal'],
       [5.5, 1.5, 2.5, 2.5],
       detalleData
     ) + '\n';

     // Separador
     ticket += PrintHelpers.Separator('-') + '\n';

     // Totales
     ticket += PrintHelpers.PrintRight([`Subtotal: ${data.data_invoice.subtotal.toFixed(2)}`]);
     ticket += PrintHelpers.PrintRight([`Impuesto: ${data.data_invoice.impuesto.toFixed(2)}`]);
     ticket += PrintHelpers.PrintRight([`Total: ${data.data_invoice.total.toFixed(2)}`]);
     ticket += PrintHelpers.PrintRight([`Pagado: ${data.data_invoice.pagado.toFixed(2)}`]);
     ticket += PrintHelpers.PrintRight([`Vuelto: ${data.data_invoice.vuelto.toFixed(2)}`]);

     // Separador
     ticket += PrintHelpers.Separator('-') + '\n';

     // Pie de página
     data.data_footer.lines.forEach(line => {
       ticket += PrintHelpers.PrintCenter([line]) + '\n';
     });

     return ticket;
   };

   export default template1;
   ```

## Ejemplo

```html
<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Ticket Printer Example</title>
    <script type="module">
      import ticketPrinter from './src/ticketPrinter.js';

      const data = {
        "data_header": {
          "lines": [
            "Nombre de la Empresa",
            "Dirección de la Empresa",
            "Teléfono: 123-456-7890",
            "Email: info@empresa.com"
          ]
        },
        "data_client": {
          "nombre": "Juan Pérez",
          "identificacion": "123456789",
          "direccion": "Calle Principal #123",
          "telefono": "555-1234",
          "email": "juan.perez@example.com"
        },
        "data_invoice": {
          "numero_factura": "F001",
          "fecha_emision": "2023-06-15",
          "metodo_pago": "Efectivo",
          "subtotal": 100.00,
          "impuesto": 15.00,
          "total": 115.00,
          "pagado": 120.00,
          "vuelto": 5.00
        },
        "data_invoice_items": [
          {
            "descripcion": "Producto 1",
            "cantidad": 2,
            "precio_unitario": 25.00,
            "subtotal": 50.00
          },
          {
            "descripcion": "Producto 2",
            "cantidad": 1,
            "precio_unitario": 50.00,
            "subtotal": 50.00
          }
        ],
        "data_footer": {
          "lines": [
            "Gracias por su compra",
            "Lo esperamos en su próxima visita",
            "Horario de atención: Lunes a Viernes de 9:00 a 18:00",
            "Síguenos en nuestras redes sociales"
          ]
        }
      };

      async function printTicket() {
        await ticketPrinter.printData(data, 'template1');
      }

      window.onload = function() {
        document.getElementById('printButton').addEventListener('click', printTicket);
      };
    </script>
  </head>
  <body>
    <h1>Ticket Printer Example</h1>
    <button id="printButton">Print Template 1</button>
  </body>
</html>
```
