; Initialize registers and perform basic arithmetic
MOV AX, 0x1234    ; Load immediate value into AX
MOV BX, 0x5678    ; Load immediate value into BX
MOV CX, AX        ; Copy result to CX
ADD CX, BX        ; Add BX to AX (AX = AX + BX)
SUB CX, 0x0100    ; Subtract 256 (0x0100) from CX

MOV AL, 0xFF      ; Load FF into lower byte of AX
MOV AH, 0x12      ; Load 12 into upper byte of AX
MOV BL, AL        ; Copy AL to BL