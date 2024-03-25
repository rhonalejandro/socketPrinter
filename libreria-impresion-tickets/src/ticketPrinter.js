// ticketPrinterTemplate1.js
import PrintHelpers from './printHelpers.js';

class ticketPrinter {
   static async getTicket(data, templateName) {
      try {
         const template = await import(`../templates/${templateName}.js`);
         return { ok: true, message: template.default(data, PrintHelpers) };
      } catch (error) {
         return { ok: false, message: `Error al cargar la plantilla ${templateName}:`, error: error.message };
      }
   }

   static async printData(data, templateName) {
      try {
         const documento = await ticketPrinter.getTicket(data, templateName);
         if (documento.ok) {
            const jsonToPrint = {
               type: 'pos-printer',
               printer: 'pos',
               data: [documento.message]
            };
            const response = await ticketPrinter.sendToSocket(JSON.stringify(jsonToPrint));
            return { ok: true, message: response };
         } else {
            return { ok: false, message: documento.message, error: documento.error };
         }
      } catch (error) {
         return { ok: false, message: 'Error al imprimir el ticket', error: error.message };
      }
   }

   static async sendToSocket(dataPrinter) {
      return new Promise((resolve, reject) => {
         let socket;
         try {
            socket = new WebSocket('ws://localhost:4500/websession');
         } catch (error) {
            reject(new Error('Error al intentar de conectar con el servicio Soket de la impresora de etiqueta.'));
            return;
         }

         socket.onopen = function () {
            socket.send(dataPrinter);
         };

         socket.onmessage = function (evt) {
            const received_msg = evt.data;
            socket.close();
            resolve(received_msg);
         };

         socket.onerror = function (error) {
            socket.close();
            reject(new Error('Error en la comunicación con el servicio Soket de la impresora de etiqueta.'));
         };

         socket.onclose = function (event) {
            if (!event.wasClean) {
               reject(new Error('La conexión con el servicio Soket de la impresora de etiqueta se cerró inesperadamente.'));
            }
         };
      });
   }
}

export default ticketPrinter;