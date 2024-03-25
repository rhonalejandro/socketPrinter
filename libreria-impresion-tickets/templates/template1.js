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
   ticket += PrintHelpers.PrintLeft([`Identificacion: ${data.data_client.identificacion}`]);
   ticket += PrintHelpers.PrintLeft([`Direccion: ${data.data_client.direccion}`]);
   ticket += PrintHelpers.PrintLeft([`Telefono: ${data.data_client.telefono}`]);
   ticket += PrintHelpers.PrintLeft([`Email: ${data.data_client.email}`]);

   // Separador
   ticket += PrintHelpers.Separator('-') + '\n';

   // Detalles de la factura
   ticket += PrintHelpers.PrintLeft([`Numero de factura: ${data.data_invoice.numero_factura}`]);
   ticket += PrintHelpers.PrintLeft([`Fecha de emision: ${data.data_invoice.fecha_emision}`]);
   ticket += PrintHelpers.PrintLeft([`Metodo de pago: ${data.data_invoice.metodo_pago}`]);

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
    ['Descripcion', 'Cant.', 'Precio', 'Subtotal'],
    [5.5, 1.5, 2.5, 2.5],
    detalleData
  ) + '\n';

   // Separador
   ticket += PrintHelpers.Separator('-') + '\n';

   // Totales
   ticket += PrintHelpers.PrintRight([`'Subtotal: ${data.data_invoice.subtotal.toFixed(2)}`]);
   ticket += PrintHelpers.PrintRight([`'Impuesto: ${data.data_invoice.impuesto.toFixed(2)}`]);
   ticket += PrintHelpers.PrintRight([`'Total:' ${data.data_invoice.total.toFixed(2)}`]);
   ticket += PrintHelpers.PrintRight([`'Pagado:' ${data.data_invoice.pagado.toFixed(2)}`]);
   ticket += PrintHelpers.PrintRight([`'Vuelto:' ${data.data_invoice.vuelto.toFixed(2)}`]);

   // Separador
   ticket += PrintHelpers.Separator('-') + '\n';

   // Pie de página
   data.data_footer.lines.forEach(line => {
      ticket += PrintHelpers.PrintCenter([line]) + '\n';
   });

   return ticket;
};

export default template1;