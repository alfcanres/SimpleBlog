function showLoadingSpinner() {

    $("#loadingSpinner").show();
}
function hideLoadingSpinner() {
    $("#loadingSpinner").hide();
}
function fnRefreshComments(postId) {
    const divCommentsList = document.getElementById("divCommentsList");
    const url = '/Post/ListComments?postId=' + postId;
    const promise = $.get(url);

    showLoadingSpinner();

    promise.then(
        function (data) {
            divCommentsList.innerHTML = data;
        },
        function (error) {

            divCommentsList.innerHTML = "<p>Something went wrong. Please try again later.</p>";
        }
    );

    hideLoadingSpinner();
}
function fnCreateComment() {
    const commentForm = document.getElementById("CommentForm");
    const commentText = commentForm.elements["CommentText"];
    const postId = commentForm.elements["PostId"].value;
    const applicationUserInfoId = commentForm.elements["ApplicationUserInfoId"].value;
    const btnSubmitComment = document.getElementById("btnSubmitComment");
    const url = commentForm.action;

    fnHideCommentValidator();

    btnSubmitComment.disabled = true;
    btnSubmitComment.value ="Submiting comment...";

    if (commentText.value === "" || commentText.value.length < 4) {
        

        fnShowCommentValidator(["Please enter a comment."]);
        commentText.focus();
        btnSubmitComment.disabled = false;
        btnSubmitComment.value = "Submit Comment";
    }
    else {
        const comment =
        {
            postId: postId,
            applicationUserInfoId: applicationUserInfoId,
            commentText: commentText.value
        };

        const promise = $.post(url, comment);



        promise.then(
            function (data) {


                if (data.status === "Success") {
                    fnRefreshComments(postId);
                  
                    commentText.value = "";
                }
                else if (data.status === "Error") {


                    fnShowCommentValidator(data.messages);
                }

                btnSubmitComment.disabled = false;               
                btnSubmitComment.value = "Submit Comment";
            },
            function (error) {

                fnShowCommentValidator(["Something went wrong. Please try again later."]);
            }
        );
    }

}
function fnShowCommentValidator(messages) {
    const validationSummary = document.getElementById("CommentValidationSummary");
    validationSummary.innerHTML = ""; 
    messages.forEach(message => {
        const li = document.createElement("li");
        li.innerText = message;
        validationSummary.appendChild(li);

    });
    $("#divValidationSummaryContainer").show();
}
function fnHideCommentValidator() {
    const validationSummary = document.getElementById("CommentValidationSummary");
    validationSummary.innerHTML = "";
    $("#divValidationSummaryContainer").hide();

}
function fnSubmitVote(vote) {


    const postId = $("#votePostId").val();
    const applicationUserInfoId = 0;
    const url = "/Post/SubmitVote";
    const voteData =
    {
        postId: postId,
        applicationUserInfoId: applicationUserInfoId,
        iLikedThis: vote
    };

    showLoadingSpinner();

    $.post(url, voteData).then(
        function (data) {
            if (data.status === "Success") {
                
                fnRefreshVoteCount(postId);
       
            }
            else if (data.status === "Error") {
                let errors = "";

                data.messages.forEach(message => {
                    errors += message;
                });

                alert(errors);
            }
        },
        function (error) {
            alert("Something went wrong. Please try again later.");
        }
    );

    hideLoadingSpinner();
}
function fnRefreshVoteCount(postId) {
    const divCommentsList = document.getElementById("divShowVotes");
    const url = '/Post/ShowVotes?postId=' + postId;
    const promise = $.get(url);

    showLoadingSpinner();

    promise.then(
        function (data) {
            divCommentsList.innerHTML = data;
        },
        function (error) {

            divCommentsList.innerHTML = "<p>Something went wrong. Please try again later.</p>";
        }
    );

    hideLoadingSpinner();
}

