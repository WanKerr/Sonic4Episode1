export default class Guide {
    public static beginShowMessageBox(playerIndex: number, title: string, text: string, buttons: string[], focusButton: number, icon: number, callback: (button: number) => void) {
        const state = { callback };

        const messageBox = document.createElement('dialog');
        messageBox.className = 'message-dialog';
        messageBox.innerHTML = `
            <div class="message-dialog-content-root">
                <div class="message-dialog-content-container">
                    <h1 class="message-dialog-title">${title}</h1>
                    <p class="message-dialog-content">${text}</p>
                    <div class="message-dialog-button-container">
                        ${buttons.map((button, index) => `<button id="button-${index}" ${index === focusButton ? 'autofocus class="message-dialog-button primary"' : 'class="message-dialog-button"'}>${button}</button>`).join('')}
                    </div>
                </div>
            </div>
        `;

        const close = (btn?: number) => {
            if (btn === undefined) {
                state.callback(-1);
            }
            else {
                state.callback(btn);
            }

            const contentRoot = messageBox.querySelector('.message-dialog-content-root') as HTMLElement;
            contentRoot.addEventListener('animationend', () => {
                messageBox.remove();
            });

            messageBox.classList.add('closing');
        }

        buttons.forEach((_, index) => {
            const button = messageBox.querySelector(`#button-${index}`) as HTMLButtonElement;
            button.addEventListener('click', () => {
                close(index);
            });
        });

        messageBox.addEventListener('close', () => {
            close();
        });

        messageBox.addEventListener('cancel', () => {
            close();
        });

        document.body.appendChild(messageBox);
        messageBox.showModal();
    }
}