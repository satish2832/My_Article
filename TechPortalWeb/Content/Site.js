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