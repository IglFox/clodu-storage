import * as mammoth from 'mammoth';
import * as pdfjsLib from 'pdfjs-dist';

// Configure pdfjs worker
pdfjsLib.GlobalWorkerOptions.workerSrc = `//cdnjs.cloudflare.com/ajax/libs/pdf.js/${pdfjsLib.version}/pdf.worker.min.js`;

export const ParserService = {
  /**
   * Parses text from various file types
   */
  async parseFile(file) {
    const extension = file.name.split('.').pop().toLowerCase();
    
    try {
      if (extension === 'pdf') {
        return await this.parsePDF(file);
      } else if (extension === 'docx') {
        return await this.parseDocx(file);
      } else if (['txt', 'md', 'json', 'js', 'html', 'css'].includes(extension)) {
        return await file.text();
      }
    } catch (e) {
      console.warn('Text extraction failed:', e);
      return null;
    }
    
    return null; // Unsupported for text extraction
  },

  /**
   * Parse PDF content
   */
  async parsePDF(file) {
    const arrayBuffer = await file.arrayBuffer();
    const loadingTask = pdfjsLib.getDocument({ data: arrayBuffer });
    const pdf = await loadingTask.promise;
    let fullText = '';
    
    for (let i = 1; i <= pdf.numPages; i++) {
      const page = await pdf.getPage(i);
      const textContent = await page.getTextContent();
      const pageText = textContent.items.map(item => item.str).join(' ');
      fullText += pageText + '\n';
    }
    
    return fullText;
  },

  /**
   * Parse Word/Docx content
   */
  async parseDocx(file) {
    const arrayBuffer = await file.arrayBuffer();
    const result = await mammoth.extractRawText({ arrayBuffer });
    return result.value;
  }
};
