export class Role  {
    public label: string;
    public value: string;
    constructor(obj?: string) {
      this.label = obj && obj || '';
      this.value = obj && obj || '';
    }
  }
