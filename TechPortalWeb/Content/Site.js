$(".btnBookDemo").on('click', function () {
    var divBookDemo = new bootstrap.Modal(document.getElementById('divBookDemo'), {
        backdrop: 'static',
        keyboard: false
    });
    setSkillset();
    $("#divEnquiryModalBody").show();
    $("#divEnquiryFooter").show();
    $("#divEnquirySuccess").hide();
    divBookDemo.show();
});

function setSkillset() {
    // AJAX request
    $.ajax({
        url: '/Home/GetSkillset',
        type: 'GET',
        contentType: 'application/json',
        success: function (response) {
            var skillSets = response.data;
            var $select = $('#enquiry-skillset');

            // Clear existing options
            $select.empty();

            // Add a default option
            $select.append($('<option>', {
                value: '',
                text: 'Select a Skillset'
            }));

            // Populate the select element with options
            $.each(skillSets, function (index, skill) {
                $select.append($('<option>', {
                    value: skill.id,
                    text: skill.name
                }));
            });
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
            alert('An error occurred while GetSkillset. Please try again.');
        }
    });
}

$('div[data-course-details-url],li[data-course-details-url]').on('click', function () {
    var url = $(this).attr("data-course-details-url");
    document.location.href = "/course/" + url;
});

$(document).ready(function () {
    $('#btnDetailsForm').on('click', function (e) {
        e.preventDefault();

        var formData = {
            name: $('#enquiry-name').val(),
            phoneNumber: $('#enquiry-phone').val(),
            email: $('#enquiry-email').val(),
            skillset: $('#enquiry-skillset option:selected').val(),
            comments: $('#enquiry-comments').val()
        };

        if (!formData.name || !formData.phoneNumber || !formData.email || !formData.skillset) {
            alert('Please fill out all required fields.');
            return;
        }

        // AJAX request
        $.ajax({
            url: '/Home/SubmitEnquiry',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                $('#detailsForm')[0].reset();
                $("#divEnquiryModalBody").hide();
                $("#divEnquiryFooter").hide();
                $("#divEnquirySuccess").show();
            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
                alert('An error occurred while submitting your details. Please try again.');
            }
        });
    });
});

$(document).ready(function () {
    $('#refreshButton').on('click', function (e) {
        $('#loadingIcon').show();
        $.ajax({
            url: '/Admin/GetEnquiryData', // API endpoint
            method: 'GET',
            success: function (data) {
                // Clear the table body
                $('#enquiryTable tbody').empty();

                // Populate the table with new data
                data.forEach(function (enquiry) {
                    $('#enquiryTable tbody').append(`
                            <tr>
                                <td>${enquiry.name}</td>
                                <td>${enquiry.phoneNumber}</td>
                                <td>${enquiry.email}</td>
                                <td>${enquiry.skillset}</td>
                                <td>${enquiry.comments}</td>
                                <td class='actions'>
                                    <a href="/admin/candidate/${enquiry.id}"><i class="fa fa-edit" aria-hidden="true"></i></a>                          
                                </td>
                            </tr>
                        `);
                });
                $('#loadingIcon').hide();
            },
            error: function () {
                $('#loadingIcon').hide();
                alert('Failed to refresh the data. Please try again later.');
            }
        });
    });
});

$(document).ready(function () {
    $(document).on('click', '#saveFollowUpButton', function (e) {
        var content = $("#followUpText").val().trim();
        if (content === "") {
            $("#errorMessage").text("Follow-up details cannot be empty.").show();
            return;
        }
        var formData = {
            candidateEnquiryId: $('#hdnCandidateEnquiryId').val(),
            content: content
        };

        $("#errorMessage").hide();
        $.ajax({
            url: '/Admin/SaveFollowup',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                // Append new follow-up to the list
                if ($("#txt-no-followups").val() != undefined) {
                    $("#txt-no-followups").empty();
                }
                var candidateEnquiryFollowup = response.data;
                $("#followUpList").prepend(`
                        <li class="list-group-item">
                            <div><strong>Last Updated By:</strong> ${candidateEnquiryFollowup.lastUpdatedBy}</div>
                            <div><strong>Content:</strong> ${candidateEnquiryFollowup.content}</div>
                            <div class="text-muted"><small><strong>Date:</strong> ${formatDate(candidateEnquiryFollowup.lastUpdatedOn)}</small></div>
                        </li>
                    `);
                $("#followUpText").val(""); // Clear text area
            },
            error: function () {
                $("#errorMessage").text("An error occurred while saving the follow-up. Please try again.").show();
            }
        });
    });

});

function formatDate(date) {
    var lastUpdatedOn = new Date(date);

    // Format the date as short date + short time
    var formattedDate = lastUpdatedOn.toLocaleString('en-US', {
        year: 'numeric',
        month: 'numeric',
        day: 'numeric',
        hour: 'numeric',
        minute: 'numeric',
        hour12: true // For 12-hour clock format
    }).replace(',', '');
    return formattedDate;
}

$(document).ready(function () {
    let firstNavLink = $('.admin-container .nav-item:first-child .nav-link');
    if (!firstNavLink || firstNavLink.length == 0) {
        return;
    }
    const currentUrl = window.location.pathname;
    const defaultAdminUrl = '/admin/enquiry-list'
    // Dynamically load the content based on the current URL
    if (currentUrl.startsWith('/admin/')) {
        loadPageContent('.admin-container .nav-item:first-child .nav-link', '.admin-container', currentUrl);       
    }
    if (currentUrl == '/admin' || currentUrl == '/admin/') {
        loadPageContent('.admin-container .nav-item:first-child .nav-link', '.admin-container', defaultAdminUrl);
    } 
    // Handle sidebar link click   
    $(document).on('click', '.admin-container .nav-link', function (e) {
        e.preventDefault(); // Prevent default link behavior

        // Remove active class from all links and add it to the clicked one
        $('.admin-container .nav-link').removeClass('active');
        $(this).addClass('active');
        loadPageContent(this, '.admin-container');
    });
    $(document).on('click', '.table-data a.data-url', function (e) {
        e.preventDefault();
        loadPageContent(this, '.admin-container');
    });
});

$(document).ready(function () {
    let firstNavLink = $('.admin-candidate-container .nav-item:first-child .nav-link');
    if (!firstNavLink || firstNavLink.length == 0) {
        return;
    }
    loadPageContent('.admin-candidate-container .nav-item:first-child .nav-link', '.admin-candidate-container');
    // Handle sidebar link click
  
    $(document).on('click', '.admin-candidate-container .nav-link', function (e) {
        e.preventDefault(); // Prevent default link behavior

        // Remove active class from all links and add it to the clicked one
        $('.admin-candidate-container .nav-link').removeClass('active');
        $(this).addClass('active');
        loadPageContent(this, '.admin-candidate-container');
    });
});

function loadPageContent(selector, parentSelector, pageUrl) {
    // Get the URL from the data attribute
    const url = pageUrl ? pageUrl : $(selector).data('url');
    if (!url) {
        return;
    }
    // Load content asynchronously
    $.ajax({
        url: url,
        method: 'GET',
        success: function (data) {
            // Load the fetched content into the right-side panel
            $(`${parentSelector} #content-area`).html(data);
            if (history.pushState) {
                history.pushState(null, '', url);
            }
        },
        error: function (error) {
            $(`${parentSelector} #content-area`).html('<p class="text-danger">Failed to load content. Please try again.</p>');
        }
    });
}

$(document).ready(function () {
    let selectedFiles = []; // Array to hold selected files
    // Handle file input change event
    $(document).on('change', '#file-input', function () {
        const files = Array.from(this.files);

        // Add new files to the selected files array
        files.forEach((file, index) => {
            if (file.type.startsWith('image/')) {
                selectedFiles.push(file);
                addPreview(file, selectedFiles.length - 1); // Add preview for the new file
            } else {
                alert('Only image files are allowed.');
            }
        });

        // Update the file input to match selectedFiles
        updateFileInput();
    });

    // Add image preview
    function addPreview(file, index) {
        const reader = new FileReader();

        reader.onload = function () {
            // Create a container for the image and remove button
            const $previewItem = $(`
                <div class="preview-item" data-index="${index}">
                    <img src="${reader.result}" alt="Preview" />
                    <button type="button" class="remove-btn" title="Remove Image">&times;</button>
                </div>
            `);
            const $preview = $('#preview');
            // Append the preview item to the preview container
            $preview.append($previewItem);
        };

        reader.readAsDataURL(file); // Read the file as a data URL
    }

    // Handle remove button click event
    $(document).on('click', '.remove-btn', function () {
        const $previewItem = $(this).closest('.preview-item');
        const index = parseInt($previewItem.data('index'));

        // Remove the file from the selected files array
        selectedFiles.splice(index, 1);

        // Remove the preview item from the DOM
        $previewItem.remove();
        const $preview = $('#preview');
        // Update indices of remaining preview items
        $preview.find('.preview-item').each((i, item) => {
            $(item).attr('data-index', i);
        });

        // Update the file input to match selectedFiles
        updateFileInput();
    });

    // Update file input with selectedFiles array
    function updateFileInput() {
        const dataTransfer = new DataTransfer();

        // Append all files from selectedFiles array to DataTransfer
        selectedFiles.forEach(file => dataTransfer.items.add(file));
        const $fileInput = $('#file-input');
        // Assign the new FileList to the file input
        $fileInput[0].files = dataTransfer.files;
    }

    // Handle form submission
    $(document).on('click', '#btnCreateArticle', function (e) {
        e.preventDefault();
        const $fileInput = $('#file-input');
        // Collect form data
        const title = $('#article-title').val();
        const titleurl = $('#article-title-url').val();
        const articleType = $('#article-type-dropdown option:selected').val();
        const content = $('#content-editor').val();
        const files = $fileInput[0].files;
        const tags = $('#tags-hidden').val();
        // Validate inputs
        if (!title || !titleurl || !content) {
            alert('Title and Title URL and content are required!');
            return;
        }

        // Create FormData object
        const formData = new FormData();
        formData.append('Title', title);
        formData.append('TitleURL', titleurl);
        formData.append('ArticleTypeId', articleType);
        formData.append('Content', content);

        // Append files to FormData
        for (let i = 0; i < files.length; i++) {
            formData.append('Images', files[i]);
        }
        formData.append('Tags', tags);

        // Send data to the server via AJAX
        $.ajax({
            url: '/Admin/SaveArticle',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                alert('Article created successfully!');
            },
            error: function (error) {
                alert('Failed to create the article!');
                console.log(error);
            }
        });
    });
});

$(document).ready(function () {
    // Add New Article Type
    $(document).on('click', '#add-article-type', function () {
        const newName = $('#new-article-type-name').val().trim();

        if (!newName) {
            $('#add-error-message').removeClass('d-none');
            return;
        }

        $('#add-error-message').addClass('d-none');

        // Simulate AJAX call to add new article type
        $.ajax({
            url: '/admin/article-type-save', // Update with your actual endpoint
            method: 'POST',
            data: { name: newName },
            success: function (response) {
                if (response.isSaved) {
                    const newRow = `
                                <tr data-id="${response.id}">
                                    <td class="name">
                                        <span class="view-mode">${newName}</span>
                                        <input type="text" class="edit-mode form-control d-none" value="${newName}" />
                                    </td>
                                    <td class="action-buttons">
                                        <button class="btn btn-warning btn-sm edit-button">Edit</button>
                                        <button class="btn btn-success btn-sm save-button d-none">Save</button>
                                        <button class="btn btn-danger btn-sm cancel-button d-none">Cancel</button>
                                    </td>
                                </tr>
                            `;
                    $('#article-type-table').append(newRow);
                    $('#new-article-type-name').val('');
                } else {
                    alert('Failed to add article type.');
                }
            },
            error: function () {
                alert('Error occurred while adding the article type.');
            }
        });
    });

    // Edit Button Click
    $(document).on('click', '#article-type-table .edit-button', function () {
        const row = $(this).closest('tr');
        row.find('.view-mode').addClass('d-none');
        row.find('.edit-mode').removeClass('d-none');
        row.find('.edit-button').addClass('d-none');
        row.find('.save-button, .cancel-button').removeClass('d-none');
    });

    // Cancel Button Click
    $(document).on('click', '#article-type-table .cancel-button', function () {
        const row = $(this).closest('tr');
        row.find('.view-mode').removeClass('d-none');
        row.find('.edit-mode').addClass('d-none');
        row.find('.edit-button').removeClass('d-none');
        row.find('.save-button, .cancel-button').addClass('d-none');
    });

    // Save Button Click
    $(document).on('click', '#article-type-table .save-button', function () {
        const row = $(this).closest('tr');
        const id = row.data('id');
        const newName = row.find('.edit-mode').val();

        $.ajax({
            url: '/admin/article-type-save', // Update with your actual endpoint
            method: 'POST',
            data: { id: id, name: newName },
            success: function (response) {
                if (response.isSaved) {
                    row.find('.view-mode').text(newName).removeClass('d-none');
                    row.find('.edit-mode').addClass('d-none');
                    row.find('.edit-button').removeClass('d-none');
                    row.find('.save-button, .cancel-button').addClass('d-none');
                } else {
                    alert('Failed to update article type.');
                }
            },
            error: function () {
                alert('Error occurred while updating the article type.');
            }
        });
    });
});

$(document).ready(function () {
    let tags = [];

    // Add tag on Enter
    $(document).on('keypress', '#tags-input', function (e) {
        if (e.which === 13) { // Enter key
            e.preventDefault();
            const tag = $(this).val().trim();
            if (tag && !tags.includes(tag)) {
                tags.push(tag);
                updateTagsContainer();
            }
            $(this).val(''); // Clear the input
        }
    });

    // Remove tag
    $(document).on('click', '.remove-tag', function () {
        const tag = $(this).data('tag');
        tags = tags.filter(t => t !== tag);
        updateTagsContainer();
    });

    // Update the tags container and hidden input
    function updateTagsContainer() {
        const container = $('#tags-container');
        container.empty();
        tags.forEach(tag => {
            container.append(`
                    <span class="badge bg-primary me-2">
                        ${tag} 
                        <i class="fa fa-times remove-tag" data-tag="${tag}" style="cursor: pointer;"></i>
                    </span>
                `);
        });
        $('#tags-hidden').val(tags.join(',')); // Update hidden input
    }
});
