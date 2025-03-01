$(function () {
    scrollToBottom();
});

function auto_grow(element) {
    element.style.height = "auto";
    element.style.height = (element.scrollHeight) + "px";
}

function auto_grow_messageContainer(element) {
    element.style.height = "auto";
    element.style.height = (element.scrollHeight + 2) + "px";

    const computedStyle = window.getComputedStyle(element);
    const minHeight = parseFloat(computedStyle.minHeight) + 10;
    if (element.scrollHeight <= minHeight) {
        element.style.setProperty("border-radius", "1rem 0 0 1rem", "important");
    } else {
        element.style.setProperty("border-radius", "1rem 0 1rem 1rem", "important");
    }
}

function reloadProfile(id) {
    $.ajax({
        url: '/Profile/ReloadPartialProfile/' + id,
        type: 'GET',
        success: function (data) {
            $('#partialViewContainer').html(data);
        },
        error: function () {
            alert('Failed to reload content.');
        }
    });
}

function reloadFriends(username = "") {
    $.ajax({
        url: '/Profile/ReloadPartialFriends/' + username,
        type: 'GET',
        success: function (data) {
            $('.friends').html(data);
            console.log('Content replaced successfully:', username);
        },
        error: function () {
            alert('Failed to reload content.');
        }
    });
    reloadFriendRequests(username);
}

function reloadFriendRequests(username = "") {
    $.ajax({
        url: '/Profile/ReloadPartialFriendRequests/' + username,
        type: 'GET',
        success: function (data) {
            $('.friend-requests').html(data);
            console.log('Content replaced successfully:', username);
        },
        error: function () {
            alert('Failed to reload content.');
        }
    });
}

function likePost(postId, element) {
    console.log("postId: " + postId);
    $.ajax({
        type: "GET",
        url: "/Posts/Like/" + postId,
        success: function (data) {
            console.log(`Success response: ${data.result}`);
            var likes = parseInt($(element).children('#Like').text());
            var dislikes = parseInt($(element).next().children('#Dislike').text());
            if (data.result == "removed") {
                $(element).children('#Like').text(likes-1);
                $(element).children('.like').attr('src', '/img/Like.png');
            }
            else {
                if (data.result == "changed") {
                    console.log(data.result);
                    $(element).next().children('#Dislike').text(dislikes-1);
                    $(element).next().children('.dislike').attr('src', '/img/Like.png');
                }
                $(element).children('#Like').text(likes + 1);
                $(element).children('.like').attr('src', '/img/LikeFull.png');
            }
            
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error); 
            $(element).children('#Like').text("Error");
        }
    });
}

function dislikePost(postId, element) {
    console.log("postId: " + postId);
    $.ajax({
        type: "GET",
        url: "/Posts/Dislike/" + postId,
        success: function (data) {
            console.log(`Success response: ${data.result}`);
            var likes = parseInt($(element).prev().children('#Like').text());
            var dislikes = parseInt($(element).children('#Dislike').text());
            if (data.result == "removed") {
                $(element).children('#Dislike').text(dislikes-1);
                $(element).children('.dislike').attr('src', '/img/Like.png');
            }
            else {
                if (data.result == "changed") {
                    console.log(data.result);
                    $(element).prev().children('#Like').text(likes-1);
                    $(element).prev().children('.like').attr('src', '/img/Like.png');
                }
                $(element).children('#Dislike').text(dislikes + 1);
                $(element).children('.dislike').attr('src', '/img/LikeFull.png');
            }

        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
            $(element).children('#Like').text("Error");
        }
    });
}

function likeComment(postId, element) {
    console.log("postId: " + postId);
    $.ajax({
        type: "GET",
        url: "/Comments/Like/" + postId,
        success: function (data) {
            console.log(`Success response: ${data.result}`);
            var likes = parseInt($(element).children('#Like').text());
            var dislikes = parseInt($(element).next().children('#Dislike').text());
            if (data.result == "removed") {
                $(element).children('#Like').text(likes - 1);
                $(element).children('.like').attr('src', '/img/Like.png');
            }
            else {
                if (data.result == "changed") {
                    console.log(data.result);
                    $(element).next().children('#Dislike').text(dislikes - 1);
                    $(element).next().children('.dislike').attr('src', '/img/Like.png');
                }
                $(element).children('#Like').text(likes + 1);
                $(element).children('.like').attr('src', '/img/LikeFull.png');
            }

        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
            $(element).children('#Like').text("Error");
        }
    });
}

function dislikeComment(postId, element) {
    console.log("postId: " + postId); 
    $.ajax({
        type: "GET",
        url: "/Comments/Dislike/" + postId,
        success: function (data) {
            console.log(`Success response: ${data.result}`);
            var likes = parseInt($(element).prev().children('#Like').text());
            var dislikes = parseInt($(element).children('#Dislike').text());
            if (data.result == "removed") {
                $(element).children('#Dislike').text(dislikes - 1);
                $(element).children('.dislike').attr('src', '/img/Like.png');
            }
            else {
                if (data.result == "changed") {
                    console.log(data.result);
                    $(element).prev().children('#Like').text(likes - 1);
                    $(element).prev().children('.like').attr('src', '/img/Like.png');
                }
                $(element).children('#Dislike').text(dislikes + 1);
                $(element).children('.dislike').attr('src', '/img/LikeFull.png');
            }
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
            $(element).children('#Like').text("Error");
        }
    });
}

function validateForm() {
    var textArea = document.querySelector('textarea[name="Text"]');
    if (textArea.value.trim() === '') {
        alert('Please enter a comment.');
        return false;
    }
    return true;
}

function sendFriendRequest(userId, onSuccess) {
    $.ajax({
        type: "GET",
        url: "/Profile/SendFriendRequest/" + userId,
        success: function (data) {
            onSuccess();
            console.log(data.result);
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
        }
    });
}

function declineFriendRequest(userId, onSuccess) {
    $.ajax({
        type: "GET",
        url: "/Profile/DeclineFriendRequest/" + userId,
        success: function (data) {
            onSuccess();
            console.log(data.result);
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
        }
    });
}

function acceptFriendRequest(userId, onSuccess) {
    $.ajax({
        type: "GET",
        url: "/Profile/AcceptFriendRequest/" + userId,
        success: function (data) {
            onSuccess();
            console.log(data.result);
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
        }
    });
}

function cancelFriendRequest(userId, onSuccess) {
    $.ajax({
        type: "GET",
        url: "/Profile/CancelFriendRequest/" + userId,
        success: function (data) {
            onSuccess();
            console.log(data.result);
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
        }
    });
}

function removeFriend(userId, onSuccess) {
    $.ajax({
        type: "GET",
        url: "/Profile/RemoveFriend/" + userId,
        success: function (data) {
            onSuccess();
            console.log(data.result);
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
        }
    });
}

function sendMessage(event, formData) {
    event.preventDefault();
    console.log("Sending message...")
    $.ajax({
        url: "/DirectChats/SendMessage",
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            console.log("Success:", response);
            if (response) {
                $('#chatMessages').append(response);
                scrollToBottom();
            }
        },
        error: function (xhr, status, error) {
            console.error("Error:", error);
            alert('Error: ' + error);
        }
    });
}

function sendTextMessage(event) {
    event.preventDefault();
    console.log("Text message button clicked!");
    let $form = $(event.target);
    let formData = new FormData($form[0]);
    let textArea = $form.find('textarea[name="Text"]');

    if (textArea.val().trim() === '') {
        alert('Please enter a message.');
        return;
    }

    sendMessage(event, formData);
    textArea.val('').trigger('input');
}

function sendImageMessage(event) {
    event.preventDefault();
    console.log("Image message button clicked!");
    let $form = $(event.target);
    let formData = new FormData($form[0]);
    let $imageFileInput = $form.find('input[type="file"]');
    let $preview = $('#imagePreview');
    let $sendButton = $('#sendImageButton');

    if (!$imageFileInput[0].files.length) {
        alert('Please select an image to send.');
        return;
    }

    sendMessage(event, formData);
    $form[0].reset();
    $preview.hide();
    $sendButton.hide();
}

function previewImage(event) {
    let file = event.target.files[0];
    let $preview = $('#imagePreview');
    let $sendButton = $('#sendImageButton');

    if (file) {
        let reader = new FileReader();
        reader.onload = function (e) {
            $preview.attr('src', e.target.result).show();
            $sendButton.show();
        };
        reader.readAsDataURL(file);
    } else {
        $preview.hide();
        $sendButton.hide();
    }
}

function scrollToBottom() {
    const chatContainer = $('#chatMessages');
    chatContainer.scrollTop(chatContainer[0].scrollHeight);
}

function updateUrl(inputElement = "") {
    const username = inputElement.value;
    const params = new URLSearchParams(window.location.search);

    if (username) {
        params.set('username', username);
    } else {
        params.delete('username');
    }

    const newUrl = `${window.location.pathname}?${params.toString()}`;
    window.history.replaceState({}, '', newUrl);
    reloadFriends(username);
}

