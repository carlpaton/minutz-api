export class Integration  {
    public apiKey: string;
    public integrationServerUrl: string;
    constructor(key?: string, url?: string) {
      this.apiKey = key && key || '';
      this.integrationServerUrl = url && url || '';
    }
  }
