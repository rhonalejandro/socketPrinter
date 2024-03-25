const template_factura_cr = (data, PrintHelpers) => {
  let ticket = '\n\n\n';

  // Encabezado
  data.data_header.lines.forEach(line => {
    ticket += PrintHelpers.PrintCenter([line]) + '\n';
  });

  // Separador
  ticket += PrintHelpers.Separator('-') + '\n';

  // Datos del receptor
  ticket += PrintHelpers.PrintLeft([`Receptor: ${data.data_receiver.nombre}`]);
  ticket += PrintHelpers.PrintLeft([`Identificacion: ${data.data_receiver.identificacion_tipo}-${data.data_receiver.identificacion_numero}`]);

  // Separador
  ticket += PrintHelpers.Separator('-') + '\n';

  // Detalles de la factura
  ticket += PrintHelpers.PrintLeft([`Clave: ${data.data_invoice.clave}`]);
  ticket += PrintHelpers.PrintLeft([`Consecutivo: ${data.data_invoice.consecutivo}`]);
  ticket += PrintHelpers.PrintLeft([`Fecha de emision: ${data.data_invoice.fecha_emision}`]);
  ticket += PrintHelpers.PrintLeft([`Condicion de venta: ${data.data_invoice.condicion_venta}`]);
  ticket += PrintHelpers.PrintLeft([`Medio de pago: ${data.data_invoice.medio_pago.join(', ')}`]);
  ticket += PrintHelpers.PrintLeft([`Moneda: ${data.data_invoice.moneda}`]);
  ticket += PrintHelpers.PrintLeft([`Tipo de cambio: ${data.data_invoice.tipo_cambio}`]);

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
  ticket += PrintHelpers.PrintRight([`Servicios gravados: ${data.data_invoice.servicios_gravados.toFixed(2)}`]);
  ticket += PrintHelpers.PrintRight([`Servicios exentos: ${data.data_invoice.servicios_exentos.toFixed(2)}`]);
  ticket += PrintHelpers.PrintRight([`Servicios exonerados: ${data.data_invoice.servicios_exonerados.toFixed(2)}`]);
  ticket += PrintHelpers.PrintRight([`Mercancias gravadas: ${data.data_invoice.mercancias_gravadas.toFixed(2)}`]);
  ticket += PrintHelpers.PrintRight([`Mercancias exentas: ${data.data_invoice.mercancias_exentas.toFixed(2)}`]);
  ticket += PrintHelpers.PrintRight([`Mercancias exoneradas: ${data.data_invoice.mercancias_exoneradas.toFixed(2)}`]);
  ticket += PrintHelpers.PrintRight([`Total gravado: ${data.data_invoice.total_gravado.toFixed(2)}`]);
  ticket += PrintHelpers.PrintRight([`Total exento: ${data.data_invoice.total_exento.toFixed(2)}`]);
  ticket += PrintHelpers.PrintRight([`Total exonerado: ${data.data_invoice.total_exonerado.toFixed(2)}`]);
  ticket += PrintHelpers.PrintRight([`Total venta: ${data.data_invoice.total_venta.toFixed(2)}`]);
  ticket += PrintHelpers.PrintRight([`Total descuentos: ${data.data_invoice.total_descuentos.toFixed(2)}`]);
  ticket += PrintHelpers.PrintRight([`Total venta neta: ${data.data_invoice.total_venta_neta.toFixed(2)}`]);
  ticket += PrintHelpers.PrintRight([`Total impuestos: ${data.data_invoice.total_impuestos.toFixed(2)}`]);
  ticket += PrintHelpers.PrintRight([`Total comprobante: ${data.data_invoice.total_comprobante.toFixed(2)}`]);
  // Separador
  ticket += PrintHelpers.Separator('-') + '\n';

  // Observaciones
  ticket += PrintHelpers.PrintLeft([`Observaciones: ${data.data_other.observaciones}`]) + '\n';

  // Pie de página
  data.data_footer.lines.forEach(line => {
    ticket += PrintHelpers.PrintCenter([line]) + '\n';
  });

  return ticket;
};

export default template_factura_cr;