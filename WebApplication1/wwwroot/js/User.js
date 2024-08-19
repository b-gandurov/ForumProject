$(document).ready(function () {
    $('#fileUploadForm').submit(function (event) {
        event.preventDefault();

        var formData = new FormData();
        var fileInput = $('#fileInput')[0].files[0];
        formData.append('file', fileInput);

        $.ajax({
            url: '/user/UploadProfilePicture', 
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                alert('File uploaded successfully');
            },
            error: function (error) {
                alert('File upload failed');
            }
        });
    });

    $("#updateUserForm").on("submit", function (event) {
        event.preventDefault();

        var userId = $("#userId").val();
        var userData = {
            email: $("#email").val(),
            firstName: $("#firstName").val(),
            lastName: $("#lastName").val()
        };

        $.ajax({
            url: `/api/users/${userId}`,
            type: "PUT",
            contentType: "application/json",
            data: JSON.stringify(userData),
            success: function (response) {
                $("#responseMessage").text(response.message);
            },
            error: function (xhr, status, error) {
                var errorMessage = xhr.status + ': ' + xhr.statusText;
                $("#responseMessage").text("Error - " + errorMessage);
            }
        });
    });
});

function ShowDeleteUser() {
    $('#deleteUser').removeClass('d-none');
}

function ShowUnblockUser() {
    $('#unblockUser').removeClass('d-none');
}

function ShowBlockUser() {
    $('#blockUser').removeClass('d-none');
}

function ShowDemoteUser() {
    $('#demoteUser').removeClass('d-none');
}

function ShowPromoteUser() {
    $('#promoteUser').removeClass('d-none');
}