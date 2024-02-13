// Inform the background page that 
// this tab should have a page-action.
chrome.runtime.sendMessage({
    from: 'content',
    subject: 'showPageAction',
});

// Listen for messages from the popup.
chrome.runtime.onMessage.addListener((msg, sender, response) => {
    if ((msg.from === 'popup') && (msg.subject === 'articleContent')) {
        var articleInfo = {
            content: document.querySelector('div [data-message-author-role="assistant"]').innerHTML,
        };

        response(articleInfo);
    }
});