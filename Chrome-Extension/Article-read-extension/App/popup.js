// Update the relevant fields with the new data.
const callBackResponseFromContent = info => {

    $.ajax({
        url: 'http://localhost:64900/api/content/save',
        type: 'POST',
        data: { text: encodeURIComponent(info.content) },
        success: function (response) {
            console.log(response);
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });

    document.getElementById('txtAreaArticle').textContent = info.content;
};

// Once the DOM is ready...
window.addEventListener('DOMContentLoaded', () => {
    // ...query for the active tab...

    document.getElementById('btnRead').addEventListener("click", () => {

        chrome.tabs.query({
            active: true,
            currentWindow: true
        }, tabs => {
            // ...and send a request for the DOM info...
            chrome.tabs.sendMessage(tabs[0].id, { from: 'popup', subject: 'articleContent' }, callBackResponseFromContent);
        });

    })

});

