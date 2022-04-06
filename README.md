# actions-smoke-test

Very simple action to be used to run a smoke test on an application.

The action will :
- Download the sitemap specified in input
- Check for all "loc" object
- Try to download each page

## Configuration
```yaml
- name: Run Smoke Test
  uses: prism-be/actions-smoke-test@main
  with:
    sitemap: 'https://url.tothesitemap.com/sitemap'
```
