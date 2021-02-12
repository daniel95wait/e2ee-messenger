# End-to-End Encrypted Instant Messenger
An end-to-end encrypted instant messaging program created as coursework

## Writeup
The main project writeup document is [`writeup.docx`](./writeup/writeup.docx) which is a Microsoft Word document. Since this is a binary file, you cannot view the changes for each commit. Therefore, [`writeup.md`](./writeup/writeup.md) is created which lacks some of the document's formatting but enables the text changes for each commit to be viewed.

The markdown file is generated using [Pandoc](https://pandoc.org/) in [`update_markdown.cmd`](./writeup/update_markdown.cmd) with the following command:

```shell
pandoc writeup.docx --standalone --extract-media . --to gfm+smart --wrap=none --reference-links -o writeup.md
```