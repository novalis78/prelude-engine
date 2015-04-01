# from pytesser import *

# image = Image.open('921eb5.png')  # Open image object using PIL
# print image_to_string(image)     # Run tesseract.exe on image
# print image_file_to_string('921eb5.png')

# from pytesser import *
# im = Image.open('921eb5.png')
# text = image_to_string(im)
# print text
import os
import Image
from PIL import Image
import pytesseract



mode = ""
current_mode = "bot"
bot_text = ""
usr_text = ""
all_text = ""
text_lines = []

filenames = next(os.walk("/home/novalis78/projects/googlescraper/GoogleScraper/examples/images"))[2]
for fileName in filenames:
	filename, fileExtension = os.path.splitext(fileName)
	#filename = '921eb5'
	text = pytesseract.image_to_string(Image.open("/home/novalis78/projects/googlescraper/GoogleScraper/examples/images/" +filename+".png"))
	text = text.split('\n')
	cnt = 0
	for line in text:
		cnt += 1
		if cnt < 5:
			continue

		if line:
			line = line.rstrip()
			#print str(cnt) + " " + line
			if "Stranger:" in line:
				bot_text += " "+line.replace("Stranger:", "")
				mode = "bot" 
				if usr_text:
					all_text = "<USER>"+usr_text+"</USER>"
					usr_text = ""
				continue
				#print mode
				#accumulateMind(mode, line)
			if "You:" in line or "Yo ." in line or "Yo " in line:
				usr_text += " "+line.replace("You:", "").replace("Yo .", "").replace("Yo ", "")
				mode = "usr"
				if bot_text:
					all_text += "<BOT>"+bot_text+"</BOT>"
					text_lines.append(all_text)
					all_text = ""
					bot_text = ""
				continue
				#print mode
				#accumulateMind(mode, line)
			if usr_text:
				usr_text += " "+line.replace("You:", "").replace("Yo .", "").replace("Yo ", "")
				continue
			if bot_text:
				bot_text += " "+line.replace("Stranger:", "")
				continue


	#print text_lines

	thefile = open(filename+'.txt', 'w') 
	for item in text_lines:
	  thefile.write("%s\n" % item)

	print "Saved to file: " + filename
