Currently relies on a Brother QL-570 set up with the default printer name and a roll of continuous tape. Means now I can print random labels with 

```
curl --data "{ title: 'this is a test title', text: 'this is some test text', images:['https://assets-cdn.github.com/images/modules/logos_page/GitHub-Mark.png']}" -H "content-type:application/json" http://localhost:9000/
```
