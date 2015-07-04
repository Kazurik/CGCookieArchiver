# CGCookieArchiver
This is a simple tool used to create a local backup of the now dieing/archived version of CGCookie.com

# Required Files
In the working directory of the program you need a credentials.txt file with your username and password on the first 2 lines.

Additionally you will need a bookmarks.html in the working directory. This is a simple file that should resemble the file created by a chrome bookmark export.

Example credentials.txt:
```
username
SuperSecurePassword
```

Example bookmarks.html:
```
<!DOCTYPE NETSCAPE-Bookmark-file-1>
<!-- This is an automatically generated file.
     It will be read and overwritten.
     DO NOT EDIT! -->
<META HTTP-EQUIV="Content-Type" CONTENT="text/html; charset=UTF-8">
<TITLE>Bookmarks</TITLE>
<H1>Bookmarks</H1>
<DL><p>
    <DT><H3 ADD_DATE="1436036482" LAST_MODIFIED="1436036647" PERSONAL_TOOLBAR_FOLDER="true">Bookmarks bar</H3>
    <DL><p>
        <DT><A HREF="https://cgcookiearchive.com/blender/cgc-courses/blender-animation-fundamentals/" ADD_DATE="1436036532" ICON="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAADE0lEQVQ4jW1TXYiUZRR+zvl+Z5xvdmd2dHe2XTBUoqsIaktMCSyji0KFLcqr9iLol3AjpYQ+qQuhpL2qiyQoasE1iFIMK0qsiE2lktXVdDdNzdxh9mdmvvl+5nvf00XNquFzdc7Fec55znkOcCPo+mRscGmu6hfzH28p5gEwbgL6XywAcOaNZRsd5i2JkjVLbMMOYi1BrKcbMcbO/tn8aGhfo9IuMq8jkKlthQ7Oux90ZGizQUCcEpRK4bkGSnm75BrpwJFzXACww/fBvg9N+LctnffhsNVzYKlnrJ9t6hZApFVqWJk8zPKd4liGjH//bbjpvZnVjQQT7YlZfDABoo2e7SXPWF8NdCIgU+vUtLIewuxy/PbHgrp0dc44PIV3GwkmxgCjLdckH/qiny9qg15sxKIBMkmEYDhi9t1HX/6iZf/JGE/cXpNecy4HgAf/K17cQcTOAwWbCnHSUpZpcsgFMd0c8T2v4OGBDnp+xSqOTws9vbZzA1BxAYQ3EFjMKzOOKUH3WvVrxeXL1Yg2dB4HWw5u7S7L8NaXdFqZ5KZz/oD4lQgnwdgHhfZtmZQhRLhl4whO2fdj76GjcNUCWtNfoxZq2vn6Dry1+x2SVr1MPjTGrklgAAhimk9ThXD2Im9+9CEZfm5IlFJIz3wBSepopcqMmnVZ1x8Njg7ld9/Rg0zbQwwAdtY73FiYl/D3r7i3v8z3DtyFFJaoK0eh/voZnPcomDwoy5wmd+XcrSeuXrsCC0Artk9NxNr8zp3ay9H8XBoGdYJOQDqR+MddiI6MiJnvpgtrPlH9Jbv1/pPetmN+b1YA4p0AASQ16nx5YeZCpA4+ZfLkaMtAChRWQZEl0d+TBNNF19wPKGaAx+72Xj17KXqEAFl0IgEy/lrfprJTH83a7M7e9qwssUk1T30ORwKYSZVcQxliefjmdLzn8T2zzwigFp9JfDD50Ptf6BtY3iVvZnId63JpxbFYQ8FCkDKu1OTyT+fCt4c/nR+56fu2SQDgw0Gs9oqlB7s7DWemltL4dDK961D9MwDV9sQA8A9tXWAM4LGKTQAAAABJRU5ErkJggg==">Blender Animation Fundamentals - Blender Cookie</A>
    </DL><p>
    <DT><A HREF="https://cgcookiearchive.com/blender/cgc-courses/sculpting-a-figure-in-blender/" ADD_DATE="1436036647" ICON="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAADE0lEQVQ4jW1TXYiUZRR+zvl+Z5xvdmd2dHe2XTBUoqsIaktMCSyji0KFLcqr9iLol3AjpYQ+qQuhpL2qiyQoasE1iFIMK0qsiE2lktXVdDdNzdxh9mdmvvl+5nvf00XNquFzdc7Fec55znkOcCPo+mRscGmu6hfzH28p5gEwbgL6XywAcOaNZRsd5i2JkjVLbMMOYi1BrKcbMcbO/tn8aGhfo9IuMq8jkKlthQ7Oux90ZGizQUCcEpRK4bkGSnm75BrpwJFzXACww/fBvg9N+LctnffhsNVzYKlnrJ9t6hZApFVqWJk8zPKd4liGjH//bbjpvZnVjQQT7YlZfDABoo2e7SXPWF8NdCIgU+vUtLIewuxy/PbHgrp0dc44PIV3GwkmxgCjLdckH/qiny9qg15sxKIBMkmEYDhi9t1HX/6iZf/JGE/cXpNecy4HgAf/K17cQcTOAwWbCnHSUpZpcsgFMd0c8T2v4OGBDnp+xSqOTws9vbZzA1BxAYQ3EFjMKzOOKUH3WvVrxeXL1Yg2dB4HWw5u7S7L8NaXdFqZ5KZz/oD4lQgnwdgHhfZtmZQhRLhl4whO2fdj76GjcNUCWtNfoxZq2vn6Dry1+x2SVr1MPjTGrklgAAhimk9ThXD2Im9+9CEZfm5IlFJIz3wBSepopcqMmnVZ1x8Njg7ld9/Rg0zbQwwAdtY73FiYl/D3r7i3v8z3DtyFFJaoK0eh/voZnPcomDwoy5wmd+XcrSeuXrsCC0Artk9NxNr8zp3ay9H8XBoGdYJOQDqR+MddiI6MiJnvpgtrPlH9Jbv1/pPetmN+b1YA4p0AASQ16nx5YeZCpA4+ZfLkaMtAChRWQZEl0d+TBNNF19wPKGaAx+72Xj17KXqEAFl0IgEy/lrfprJTH83a7M7e9qwssUk1T30ORwKYSZVcQxliefjmdLzn8T2zzwigFp9JfDD50Ptf6BtY3iVvZnId63JpxbFYQ8FCkDKu1OTyT+fCt4c/nR+56fu2SQDgw0Gs9oqlB7s7DWemltL4dDK961D9MwDV9sQA8A9tXWAM4LGKTQAAAABJRU5ErkJggg==">Sculpting a Human Figure in Blender - Blender Cookie</A>
</DL><p>
```