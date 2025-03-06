$(function () {
    scrollToBottom();
    var activeChatId = sessionStorage.getItem('activeChatId');
    if (activeChatId) {
        openChat(activeChatId);
    }
});

function auto_grow(element) {
    element.style.height = "auto";
    element.style.height = (element.scrollHeight) + "px";
}

function auto_grow_commentBar(element) {
    element.style.height = "auto";
    element.style.height = (element.scrollHeight) + "px";

    const computedStyle = window.getComputedStyle(element);
    const minHeight = parseFloat(computedStyle.minHeight) + 10;
    if (element.scrollHeight <= minHeight) {
        element.style.setProperty("border-radius", "1rem 0 0 1rem", "important");
    } else {
        element.style.setProperty("border-radius", "1rem 0 1rem 1rem", "important");
    }
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
            if (response) {
                if (response.success === false) {
                    alert(response.error);
                } else {
                    console.log("Success:", response);
                    $('#chatMessages').append(response);
                    scrollToBottom();
                }
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
    const form = event.target;
    const textArea = form.querySelector('textarea[name="Text"]');

    if (textArea.value.trim() === '') {
        alert('Please enter a message.');
        return;
    }

    const formData = new FormData(form);
    sendMessage(event, formData);
    textArea.value = '';
    textArea.dispatchEvent(new Event('input'));
}

function sendImageMessage(event) {
    event.preventDefault();
    console.log("Image message button clicked!");
    const form = event.target;
    const fileInput = form.querySelector('input[type="file"]');
    const preview = document.getElementById('imagePreview');
    const sendButton = document.getElementById('sendImageButton');
    const file = fileInput.files[0];
    if (!file) {
        alert('Please select an image to send.');
        return;
    }

    if (!validateImageFile(file)) {
        return;
    }

    const formData = new FormData(form);
    sendMessage(event, formData);
    form.reset();
    preview.style.display = 'none';
    sendButton.style.display = 'none';
}

/*function deleteMessage(chatId, messageId) {
    $.ajax({
        url: '/DirectChats/DeleteMessage',
        type: 'POST',
        data: {
            chatId: chatId,
            messageId: messageId
        },
        success: function (response) {
            if (response.success) {
                $('#message-' + messageId).remove();
                console.log("Message deleted successfully!")
            } else {
                alert('Failed to delete the message');
            }
        },
        error: function (xhr, status, error) {
            console.error("Error:", error);
            alert('Error: ' + error);
        }
    });
}*/

function openChat(chatId) {
    sessionStorage.setItem('activeChatId', chatId);
    $.ajax({
        url: "/DirectChats/OpenChat?activeChatId=" + chatId,
        type: 'GET',
        success: function (result) {
            $('.left-container').html(result);
            window.history.replaceState({}, '', '/DirectChats');
            scrollToBottom();
        },
        error: function () {
            console.error("Error: ", error);
            alert('Failed to open chat.');
        }
    });
}

function previewImage(event) {
    const file = event.target.files[0];
    const preview = document.getElementById('imagePreview');
    const sendButton = document.getElementById('sendImageButton');

    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
            preview.style.display = 'block';
            sendButton.style.display = 'inline-block';
        };
        reader.readAsDataURL(file);
    } else {
        preview.style.display = 'none';
        sendButton.style.display = 'none';
    }
}

function validateImageFile(file) {
    const allowedTypes = ['image/jpeg', 'image/png', 'image/gif'];
    const maxSize = 15 * 1024 * 1024;

    if (!allowedTypes.includes(file.type)) {
        alert("Invalid file type. Only JPEG, PNG, and GIF images are allowed.");
        return false;
    }

    if (file.size > maxSize) {
        alert("File size exceeds the maximum limit of 15MB.");
        return false;
    }

    return true;
}

function scrollToBottom() {
    const chatContainer = $('#chatMessages');
    chatContainer.scrollTop(chatContainer[0].scrollHeight);
}

function clearSessionStorage() {
    sessionStorage.removeItem('activeChatId');
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

