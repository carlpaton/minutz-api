import { MinutzWebPage } from './app.po';

describe('minutz-web App', () => {
  let page: MinutzWebPage;

  beforeEach(() => {
    page = new MinutzWebPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!');
  });
});
