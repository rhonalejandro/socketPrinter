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

## Etiquetas impresoras Zebras

Este proyecto tambien es compatible con impresoras de etiquetas actualmente funciona para impresoras de etiquetas zebra.

1. Genera un .zpl y colocalo en la carpeta del socket en la raiz donde se encuentra el ejecutable.
   ejemplo:

   ```
   CT~~CD,~CC^~CT~
   ^XA
   ^DFR:EtiquetaMyS.ZPL^FS
   ~TA000
   ~JSN
   ^LT0
   ^MNW
   ^MTT
   ^PON
   ^PMN
   ^LH0,0
   ^JMA
   ^PR4,4
   ~SD15
   ^JUS
   ^LRN
   ^CI27
   ^PA0,1,1,0
   ^MMT
   ^PW909
   ^LL1559
   ^LS0
   ^FO44,49^GB820,0,12^FS
   ^FO44,1500^GB820,0,12^FS
   ^FO25,412^GB838,0,2^FS
   ^FO25,739^GB838,0,2^FS
   ^FO38,1179^GB838,0,2^FS
   ^FT44,467^A0N,38,38^FH\^CI28^FDDatos del receptor^FS^CI27
   ^FO38,82^GFA,1573,5964,28,:Z64:eJztmL9r40gUx59+DAhpGRyQ+uBtzC6Eg2vMqlHA7mWQ/p8h1wjn0N8gkkZ4j3ClkCBsedX+DWYrERbVqcy+kSXZ0kyGcCzH3ZHnQBQnn3zyfW9mJALwVm/1M8r6h33J38P0y8kbM14v/HDDa99eWlNfzCuQYkbFK2uv5xOfpuDsA6/9UZePv0cUnHPyzac+r+VCKXd1ypckk3yxwlcNPn3q0xScfmj4a3/UJa/mjJNvPvUd2yLPR5s+nyX4PIXPH3y64IsV3OHQzy9P8vH8NAXXxUMf1419PSfL18XDfG28UT6i8DmDby74PAX32HTzO+pGvljBVb1vLvj6eLJ8etPnSwQfUfj6eNWd6CMKH+3Gd6jb6Y3m5yk4X+GLFVwfr6mFfENbJPn61SLzaQof7aZ3kPiIgvMVPk/BPSjyxYp8g07cD5qC0w9D4X4Yz48oOEfhO+OEfFdDPHH/eQqfr/DFCm6Y3kHwaQrutFpE31k8IR9tXs5HFD5f4fMUXLda5PM7647Ql2O9eH72nPz+B/L5/QTfhPuwayuTza9rSyDjunYyma/rpDRfNzkmydetlkj2d3arpZTlIwrOPsbL4f85v/5OxECc39AWCef07ZT4vGHfifm6s6WW5YvH3Mh3em4Rfad9J3Ld+czgvzu/i2PNZFx3btYA4vzOjr8ppw/HmOg7f5yb5rO71fJFkq8/OmX5+jsRe00/X+jLv3h++vHpYS7zjZ5WJ1z/3MJa33h+3lk7p5w/bD7RFw+bVszX3/Uk+TSRG3z6rj8DeR1Pe9B5yTt3XgzY6Ip1L8DfHmABfwG/5h/tJ4ClcVtiqJvSKA2DVZ9Lp8h+ZZ9LowAIiRfFm1iLSEQIxOvYi4INrCOCMWuafm+emm1DGzvlAa/q/Qf2taH1wPGfJ6YWryPOXfMvAQqnKqr76rb6WBpbA31+kb3Pbku/bLmw47Qjtxm4Z5okKdt+TfNET3VsY558+bb/Ky3+AIg8ABO0FZmB5mqYMNSC9XVAZi76fAY2wNZYglEY2IxLYPd3TF/aJ05DLuy462toue9XDCjoqZ2DXuvI7YE9PTG6eMc5EzmyIi5yZGWaAfo2psm5ne8XtlFtdT9DX+mUmcHu7rf6LxbnvNAksYkx0ReTKLiA9cY1+f8NmqfHZ0oPv9PHZ9jrh/Sw/8SaMqH5AmC1bjnX9EIIND6FC1ghF3LfHfeVlv2xgMyobsvMZ/ytyyUGQ84loWviwALouIuZGaIvbRbWO1pb1FpgNDuxcHrNVU7rBWjrVQj8t5swQ05zAae+8lwzmIF+u70Ey15i+5aQYUshe892nY+YM+BpkNOQQxh9XudLa2bR5SKFAn03OdvvWEOLtM7RZ0aBa7putMIBmGs3iDZrbG4UhWD8tvX/XNqf/LJClVP5fPHsHB+XDffF4cx0cbEhR+J28az42gtB36ZNvaCU1nj/YxRXZ/OtSelD/VD3HHYlxsGRCLlrzuGXYNzYVba0baeocHAOavys2tp+7meggRnDzCRa4OE1Cb3QCzwcJnjHzdbuOax+4/E3Tnvyrd7qdfUDr349Qw==:5F10
   ^FO731,282^GB110,104,10^FS
   ^BY3,3,109^FT106,1455^BCN,,Y,N,,A
   ^FN1"tracking"^FS
   ^FT278,120^A0N,38,38^FH\^CI28^FN2"nombrecliente"^FS^CI27
   ^FT278,174^A0N,38,38^FH\^CI28^FN3"identificacioncliente"^FS^CI27
   ^FT278,288^A0N,38,38^FH\^CI28^FN4"telefonocliente"^FS^CI27
   ^FT278,230^A0N,38,38^FH\^CI28^FN5"emailcliente"^FS^CI27
   ^FT703,352^A0N,54,124^FB206,1,14,C^FH\^CI28^FN16"tipopaquete"^FS^CI27
   ^FT38,363^A0N,46,61^FH\^CI28^FN6"desdehasta"^FS^CI27
   ^FT44,1051^A0N,29,28^FH\^CI28^FN7"destino"^FS^CI27
   ^FT44,535^A0N,38,38^FH\^CI28^FN8"nombrereceptor"^FS^CI27
   ^FT44,596^A0N,38,38^FH\^CI28^FN9"identificacionreceptor"^FS^CI27
   ^FT44,645^A0N,38,38^FH\^CI28^FN10"telefonoreceptor"^FS^CI27
   ^FT44,946^A0N,38,38^FH\^CI28^FN11"descripcionpaquete"^FS^CI27
   ^FT653,825^A0N,33,33^FH\^CI28^FN12"cantidadpaquete"^FS^CI27
   ^BY6,3,76^FT120,1289^BCN,,Y,N,,A
   ^FN13"destinocodigo"^FS
   ^FT596,740^BQN,2,7
   ^FH\^FN14"rutawaze"^FS
   ^FT44,825^A0N,33,33^FH\^CI28^FN15"recibofisico"^FS^CI27
   ^FT44,1092^A0N,29,28^FH\^CI28^FN17"destinol2"^FS^CI27
   ^FT44,1134^A0N,29,28^FH\^CI28^FN18"destinol3"^FS^CI27

   ^XZ
   ```
   2. Crea un objeto con los datos necesarios para generar el ticket. Por ejemplo:

      ```
            const jsonObject = {
            "type": "label-printer",
            "printer": "zebra",
            "data": [`<file><label>
                  <variable name='tracking'>123456789</variable>
                  <variable name='recibofisico'>Recibo: ABC123</variable>
                  <variable name='nombrecliente'>Nombre del Cliente</variable>
                  <variable name='identificacioncliente'>C.I 1234567</variable>
                  <variable name='telefonocliente'>Telefono: 123-456-7890</variable>
                  <variable name='emailcliente'>Email:</variable>
                  <variable name='tipopaquete'>TIPO</variable>
                  <variable name='desdehasta'>Ruta Origen -> Ruta Destino</variable>
                  <variable name='destino'>Dirección de Destino</variable>
                  <variable name='destinol2'>Linea 2 de la Dirección</variable>
                  <variable name='destinol3'>Linea 3 de la Dirección</variable>
                  <variable name='nombrereceptor'>Nombre del Receptor</variable>
                  <variable name='identificacionreceptor'>C.I 7654321</variable>
                  <variable name='telefonoreceptor'>Telefono del Receptor</variable>
                  <variable name='descripcionpaquete'>Descripción del Paquete</variable>
                  <variable name='cantidadpaquete'>Paquete 1 de 3</variable>
                  <variable name='destinocodigo'>Código de la Ruta de Destino</variable>
                  <variable name='rutawaze'>https://waze.com/ul?ll=latitud,longitud%26z=10</variable>
               </label></file>`]
            }
      ```
      en este objeto se debe enviar el tipo de impresion "label-printer" y la impresora "zebra" (cuando inicie el proyecto antes de pensar hacerla publica trabaje con etiquetadoras q5bt con la libreria tspl - R42 y tsc con la libreria tsclib) este proyecto solo esta disponible para las zebras.

      ### Ejemplo
   3. ```html
      <!DOCTYPE html>
      <html lang="en">

         <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Zebra Printer Example</title>
            <script type="module">
               import ticketPrinter from './src/ticketPrinter.js';

            const jsonObject = {
            "type": "label-printer",
            "printer": "zebra",
            "data": [`<file><label>
                  <variable name='tracking'>123456789</variable>
                  <variable name='recibofisico'>Recibo: ABC123</variable>
                  <variable name='nombrecliente'>Nombre del Cliente</variable>
                  <variable name='identificacioncliente'>C.I 1234567</variable>
                  <variable name='telefonocliente'>Telefono: 123-456-7890</variable>
                  <variable name='emailcliente'>Email:</variable>
                  <variable name='tipopaquete'>TIPO</variable>
                  <variable name='desdehasta'>Ruta Origen -> Ruta Destino</variable>
                  <variable name='destino'>Dirección de Destino</variable>
                  <variable name='destinol2'>Linea 2 de la Dirección</variable>
                  <variable name='destinol3'>Linea 3 de la Dirección</variable>
                  <variable name='nombrereceptor'>Nombre del Receptor</variable>
                  <variable name='identificacionreceptor'>C.I 7654321</variable>
                  <variable name='telefonoreceptor'>Telefono del Receptor</variable>
                  <variable name='descripcionpaquete'>Descripción del Paquete</variable>
                  <variable name='cantidadpaquete'>Paquete 1 de 3</variable>
                  <variable name='destinocodigo'>Código de la Ruta de Destino</variable>
                  <variable name='rutawaze'>https://waze.com/ul?ll=latitud,longitud%26z=10</variable>
               </label></file>`]
            }
         

          async function printLabel() {
            await ticketPrinter.printLabel(jsonObject);
          }

          window.onload = function() {
          document.getElementById('printButton').addEventListener('click', printLabel);
          };
        </script>
         </head>

         <body>
            <h1>Ticket Printer Example</h1>
            <button id="printButton">Print Label zebra</button>
         </body>

      </html>
      ```
