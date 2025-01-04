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
    $('#btnDetailsForm').click(function (e) {
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
    $('#refreshButton').click(function () {
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
                                    <a href="/candidate/${enquiry.id}"><i class="fa fa-edit" aria-hidden="true"></i></a>                          
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
    $("#saveFollowUpButton").click(function () {
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
    $('.admin-container .nav-link').on('click', function () {
        // Remove 'active' class from all links
        $('.admin-container .nav-link').removeClass('active');

        // Add 'active' class to the clicked link
        $(this).addClass('active');
    });
});

$(document).ready(function () {
    // Handle sidebar link click
    $('.admin-candidate-container .nav-link').on('click', function (e) {
        e.preventDefault(); // Prevent default link behavior

        // Remove active class from all links and add it to the clicked one
        $('.admin-candidate-container .nav-link').removeClass('active');
        $(this).addClass('active');

        // Get the URL from the data attribute
        const url = $(this).data('url');

        // Load content asynchronously
        $.ajax({
            url: url,
            method: 'GET',
            success: function (data) {
                // Load the fetched content into the right-side panel
                $('.admin-candidate-container #content-area').html(data);
            },
            error: function () {
                $('.admin-candidate-container #content-area').html('<p class="text-danger">Failed to load content. Please try again.</p>');
            }
        });
    });
});