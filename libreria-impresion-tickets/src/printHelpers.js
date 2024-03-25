// printHelpers.js
class PrintHelpers {
   // Method to format a string with a name, price, and optional dollar sign
   // Método para formatear una cadena con un nombre, precio y signo de dólar opcional
   static getAsString(name, price, dollarSign = null, width = 48) {
      const rightCols = 10;
      let leftCols = width - rightCols;
      if (dollarSign) {
         leftCols = leftCols / 2 - rightCols / 2;
      }
      const left = name.padEnd(leftCols);

      const sign = dollarSign ? '$ ' : '';
      const right = (sign + price).padStart(rightCols);
      return `${left}${right}\n`;
   }

   // Method to print data in columns
   // Método para imprimir datos en columnas
   static PrintColumns(data) {
      const columns = PrintHelpers.getColumns(data.length);
      let result = '';
      const formattedData = data.map((item, index) => {
         if (item.length > columns[index]) {
            return item.match(new RegExp(`.{1,${columns[index]}}`, 'g'));
         }
         return item;
      });

      const rows = Math.max(...formattedData.map(item => Array.isArray(item) ? item.length : 1));
      for (let i = 0; i < rows; i++) {
         for (let j = 0; j < formattedData.length; j++) {
            const item = formattedData[j];
            const padding = ' '.repeat(columns[j] - (Array.isArray(item) ? item[i]?.length || 0 : item.length));
            result += (Array.isArray(item) ? item[i] || '' : item) + padding + ' ';
         }
         result += '\n';
      }
      return result;
   }

   // Method to print data centered
   // Método para imprimir datos centrados
   static PrintCenter(data) {
      const columns = PrintHelpers.getColumns(data.length);
      let result = '';

      data.forEach((item, index) => {
         const words = item.split(' ');
         let line = '';

         words.forEach((word) => {
            if (line.length + word.length + 1 <= columns[index]) {
               line += (line ? ' ' : '') + word;
            } else {
               const padding = ' '.repeat(Math.floor((columns[index] - line.length) / 2));
               result += padding + line + padding + (line.length % 2 === 0 ? ' ' : '');
               line = word;
            }
         });

         if (line) {
            const padding = ' '.repeat(Math.floor((columns[index] - line.length) / 2));
            result += padding + line + padding + (line.length % 2 === 0 ? ' ' : '');
         }
      });

      return result;
   }

   // Method to print a cut line
   // Método para imprimir una línea de corte
   static Cut() {
      return "x001bi0d\n";
   }

   // Method to print data left-aligned
   // Método para imprimir datos alineados a la izquierda
   static PrintLeft(data) {
      const columns = PrintHelpers.getColumns(data.length);
      let result = '';

      data.forEach((item, index) => {
         const words = item.split(' ');
         let line = '';

         words.forEach((word) => {
            if (line.length + word.length + 1 <= columns[index]) {
               line += (line ? ' ' : '') + word;
            } else {
               result += line.padEnd(columns[index]) + '\n';
               line = word;
            }
         });

         if (line) {
            result += line.padEnd(columns[index]) + '\n';
         }
      });

      return result;
   }

   static PrintRight(data) {
      const columns = PrintHelpers.getColumns(data.length);
      let result = '';

      data.forEach((item, index) => {
         const words = item.split(' ');
         let line = '';

         words.forEach((word) => {
            if (line.length + word.length + 1 <= columns[index]) {
               line = (line ? line + ' ' : '') + word;
            } else {
               result += line.padStart(columns[index]) + '\n';
               line = word;
            }
         });

         if (line) {
            result += line.padStart(columns[index]) + '\n';
         }
      });

      return result;
   }

  static PrintColumn(data, columnsSetting, dataRows = []) {
  const totalWidth = 48; // Ancho total del ticket
  const columns = [];

  // Calcular el ancho de cada columna en base a columnsSetting
  columnsSetting.forEach(setting => {
    const columnWidth = Math.floor(totalWidth * setting / 12);
    columns.push(columnWidth);
  });

  let result = '';

  // Verificar si la cantidad de elementos en data coincide con la cantidad de columnas
  if (data.length !== columns.length) {
    throw new Error('La cantidad de elementos en data debe coincidir con la cantidad de columnas.');
  }

  // Imprimir los encabezados de las columnas
  data.forEach((item, index) => {
    result += item.padEnd(columns[index]).slice(0, columns[index]);
  });
  result = result.trimEnd() + '\n';

  // Imprimir la línea separadora
  result += '-'.repeat(totalWidth) + '\n';

  // Imprimir los datos de las filas
  dataRows.forEach(row => {
    let rowResult = '';
    row.forEach((item, index) => {
      rowResult += item.toString().padEnd(columns[index]).slice(0, columns[index]);
    });
    result += rowResult.trimEnd() + '\n';
  });

  return result;
}

   // Method to print a separator line
   // Método para imprimir una línea separadora
   static Separator(character = '-', width = 48) {
      try {
         return character.repeat(width) + '\n';
      } catch (error) {
         character = '-';
         return character.repeat(width) + '\n';
      }
   }

   // Method to get the column widths based on the number of columns
   // Método para obtener los anchos de columna basados en el número de columnas
   static getColumns(numColumns) {
      const totalWidth = 48;
      const columnWidth = Math.floor(totalWidth / numColumns);
      const remainingWidth = totalWidth % numColumns;
      const columns = Array(numColumns).fill(columnWidth);
      for (let i = 0; i < remainingWidth; i++) {
         columns[i]++;
      }
      return columns;
   }
}

export default PrintHelpers;