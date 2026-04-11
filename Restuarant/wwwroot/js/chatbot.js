document.addEventListener("DOMContentLoaded", function () {

    const chatBody = document.getElementById("chatBody");
    const userInput = document.getElementById("userInput");
    const sendBtn = document.getElementById("sendBtn");
    const chatbot = document.getElementById("chatbot");

    if (!chatBody || !userInput || !sendBtn || !chatbot) {
        console.error("Chatbot elements not found");
        return;
    }

    /* Toggle chat */
    window.toggleChat = function () {
        chatbot.classList.toggle("collapsed");
    };

    /* Send button */
    sendBtn.addEventListener("click", sendMessage);

    /* Enter key */
    userInput.addEventListener("keydown", function (e) {
        if (e.key === "Enter") {
            sendMessage();
        }
    });

    function sendMessage() {
        const msg = userInput.value.trim();
        if (msg === "") return;

        addMessage(msg, "user-msg");
        userInput.value = "";

        setTimeout(() => {
            botReply(msg.toLowerCase());
        }, 500);
    }

    function addMessage(text, className) {
        const div = document.createElement("div");
        div.className = className;
        div.innerText = text;
        chatBody.appendChild(div);
        chatBody.scrollTop = chatBody.scrollHeight;
    }

    function botReply(message) {

        let reply = "I'm here to help 😊";

        if (message.includes("hi") || message.includes("hello")) {
            reply = "Hello 👋 Welcome to Savora! How can I help you?";
        }
        else if (message.includes("book")) {
            reply = "🍽️ You can reserve a table from our Booking page.";
        }
        else if (message.includes("menu")) {
            reply = "We serve authentic Indian cuisine – Veg & Non-Veg 🌶️";
        }
        else if (message.includes("location")) {
            reply = "📍 Bangalore – Whitefield & Hennur branches.";
        }
        else if (message.includes("time")) {
            reply = "🕙 We are open daily from 10 AM to 11 PM.";
        }
        else if (message.includes("contact")) {
            reply = "📞 +91 98765 43210 | ✉ homeofflavours@gmail.com";
        }

        addMessage(reply, "bot-msg");
    }

    /* Default greeting */
    setTimeout(() => {
        addMessage("👋 Hi! I’m Savora Assistant. Ask me about bookings, menu, or timings.", "bot-msg");
    }, 700);

});
