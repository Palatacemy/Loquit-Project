function auto_grow(element) {
    element.style.height = (element.scrollHeight) + "px";
}

function likePost(postId, element) {
    console.log("postId: " + postId); // Check if postId is correctly passed
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
            console.error("Error: " + error); // Check for any errors
            $(element).children('#Like').text("Error");
        }
    });
}

function dislikePost(postId, element) {
    console.log("postId: " + postId); // Check if postId is correctly passed
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
            console.error("Error: " + error); // Check for any errors
            $(element).children('#Like').text("Error");
        }
    });
}

function likeComment(postId, element) {
    console.log("postId: " + postId); // Check if postId is correctly passed
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
            console.error("Error: " + error); // Check for any errors
            $(element).children('#Like').text("Error");
        }
    });
}

function dislikeComment(postId, element) {
    console.log("postId: " + postId); // Check if postId is correctly passed
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
            console.error("Error: " + error); // Check for any errors
            $(element).children('#Like').text("Error");
        }
    });
}

