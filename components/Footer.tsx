export default function Footer() {
  const currentYear = new Date().getFullYear()

  return (
    <footer className="bg-secondary text-white py-8 mt-16">
      <div className="container mx-auto px-4">
        <div className="text-center">
          <p className="mb-2">© {currentYear} Cooking Quiz App. All rights reserved.</p>
          <p className="text-sm opacity-80">
            Test your culinary knowledge and become a cooking master!
          </p>
        </div>
      </div>
    </footer>
  )
}