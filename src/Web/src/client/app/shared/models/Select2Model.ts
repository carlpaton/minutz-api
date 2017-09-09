export class Select2Model  {
    public label: string;
    public value: string;
    constructor(label?: string, value?: string) {
      this.label = label && label || '';
      this.value = value && value || '';
    }
  }
